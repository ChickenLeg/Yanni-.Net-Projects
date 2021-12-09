using Azure;
using Azure.Storage.Blobs;
using Lab4.Data;
using Lab4.Models;
using Lab4.Models.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IO;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;

namespace Lab4.Controllers
{
    public class AdvertisementController : Controller
    {
        private readonly SchoolCommunityContext _context;
        private readonly BlobServiceClient _blobServiceClient;
        private readonly string containerName = "advertisements";

        public AdvertisementController(SchoolCommunityContext context, BlobServiceClient blobServiceClient)
        {
            _context = context;
            _blobServiceClient = blobServiceClient;
        }

        public IActionResult Index(string? Id)
        {
            var adv = new AdsViewModel();
            adv.Advertisements = new List<Advertisement>();
            if (Id == null)
            {
                Id = (from i in _context.Communities
                      select i).First().CommunityID;
            }
            var advertisement = (from c in _context.Advertisements
                                 where c.CommunityID == Id
                                 select c).ToList();
            foreach (var item in advertisement)
            {
                adv.Advertisements.Add(item);
            }
            adv.Community = (from m in _context.Communities
                             where m.CommunityID == Id
                             select m).First();
            return View(adv);
        }

        public IActionResult Create(string? id)
        {
            ViewData["CommunityId"] = id;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(IFormFile file, string? id)
        {

            BlobContainerClient containerClient;
            // Create the container and return a container client object
            try
            {
                containerClient = await _blobServiceClient.CreateBlobContainerAsync(containerName);
                // Give access to public
                containerClient.SetAccessPolicy(Azure.Storage.Blobs.Models.PublicAccessType.BlobContainer);
            }
            catch (RequestFailedException)
            {
                containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
            }


            try
            {
                string randomFileName = Path.GetRandomFileName();
                // create the blob to hold the data
                var blockBlob = containerClient.GetBlobClient(randomFileName);
                if (await blockBlob.ExistsAsync())
                {
                    await blockBlob.DeleteAsync();
                }

                using (var memoryStream = new MemoryStream())
                {
                    // copy the file data into memory
                    await file.CopyToAsync(memoryStream);

                    // navigate back to the beginning of the memory stream
                    memoryStream.Position = 0;

                    // send the file to the cloud
                    await blockBlob.UploadAsync(memoryStream);
                    memoryStream.Close();
                }
                var com = (from m in _context.Communities
                           where m.CommunityID == id
                           select m).First();
                // add the photo to the database if it uploaded successfully
                var image = new Advertisement
                {
                    Url = blockBlob.Uri.AbsoluteUri,
                    FileName = randomFileName,
                    CommunityID = com.CommunityID
                };

                _context.Advertisements.Add(image);
                _context.SaveChanges();
            }
            catch (RequestFailedException)
            {
                View("Error");
            }

            return RedirectToAction("Index", new { Id = id });
        }
        public async Task<IActionResult> Delete(int? id)
        {
            var image1 = await _context.Advertisements.FindAsync(id);
            string communityId = image1.CommunityID;
            ViewData["CommunityId"] = communityId;
            if (id == null)
            {
                return NotFound();
            }

            var image = await _context.Advertisements
                .FirstOrDefaultAsync(m => m.ID == id);
            if (image == null)
            {
                return NotFound();
            }

            return View(image);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int ID)
        {
            var image = await _context.Advertisements.FindAsync(ID);
            string communityId = image.CommunityID;
            

            BlobContainerClient containerClient;
            // Get the container and return a container client object
            try
            {
                containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
            }
            catch (RequestFailedException)
            {
                return View("Error");
            }

            try
            {
                // Get the blob that holds the data
                var blockBlob = containerClient.GetBlobClient(image.FileName);
                if (await blockBlob.ExistsAsync())
                {
                    await blockBlob.DeleteAsync();
                }

                _context.Advertisements.Remove(image);
                await _context.SaveChangesAsync();

            }
            catch (RequestFailedException)
            {
                return View("Error");
            }

            return RedirectToAction("Index", new { Id = communityId });
        }
    }
}

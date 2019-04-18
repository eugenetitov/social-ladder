using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using SocialLadder.Interfaces;
using System;
using System.IO;
using System.Threading.Tasks;

namespace SocialLadder.iOS
{

    public class CloudinaryService : ICloudinaryService
    {
        private Cloudinary _cloudinary
        {
            get; set;
        }

        public CloudinaryService()
        {
            Init();
        }

        public void Init()
        {
            Account account = new Account(
                "socialladder",
                "912913465365124",
                "L8C6ce8W52mKXgazvc1Tr96YZjs");

            _cloudinary = new Cloudinary(account);
        }

        /// <summary>
        /// Upload a file by its path.
        /// </summary>
        /// <returns>The upload end path.</returns>
        /// <param name="path">Path.</param>
        public async Task<string> Upload(string path)
        {
            ImageUploadResult uploadResult = null;
            try
            {
                ImageUploadParams uploadParams = new ImageUploadParams()
                {
                    File = new FileDescription(path),
                    PublicId = GetGuid
                };
                uploadResult = await _cloudinary.UploadAsync(uploadParams);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return uploadResult != null ? uploadResult.Uri.ToString() : null;
        }

        string GetGuid
        {
            get
            {
                return String.Format("{0}.{1}.jpg", Guid.NewGuid().ToString(), DateTime.Now.ToString("yyyy-MM-dd-HH:mm:ss")); 
            }
        }

        public async Task<string> Upload(Stream stream)
        {
            ImageUploadResult uploadResult = null;
            try
            {
                var uploadParams = new ImageUploadParams()
                {
                    File = new FileDescription(GetGuid, stream),
                    PublicId = GetGuid
                };
                uploadResult = await _cloudinary.UploadAsync(uploadParams);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return uploadResult != null ? uploadResult.Uri.ToString() : null;
        }

        public async Task<string> Upload(byte[] data)
        {
            ImageUploadResult uploadResult = null;
            try
            {
                Stream stream = new MemoryStream(data);
                var uploadParams = new ImageUploadParams()
                {
                    File = new FileDescription(GetGuid, stream),
                    PublicId = GetGuid
                };
                uploadResult = await _cloudinary.UploadAsync(uploadParams);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return uploadResult != null ? uploadResult.Uri.ToString() : null;
        }

        public async Task<string> UploadGetId(Stream stream)
        {
            ImageUploadResult uploadResult = null;
            try
            {
                var uploadParams = new ImageUploadParams()
                {
                    File = new FileDescription(GetGuid, stream),
                    PublicId = GetGuid
                };
                uploadResult = await _cloudinary.UploadAsync(uploadParams);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return uploadResult != null ? uploadResult.PublicId.ToString() : null;
        }

        /// <summary>
        /// Delete the specified url.
        /// </summary>
        /// <returns>The delete.</returns>
        /// <param name="url">URL.</param>
        public void Delete(string url)
        {
            DeletionParams deletionParams = new DeletionParams(url);
            _cloudinary.DestroyAsync(deletionParams);
        }
    }
}
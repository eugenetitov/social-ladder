//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace SocialLadder.Services
//{
//    class CloudinaryService
//    {
//        private Cloudinary cloudinary
//        {
//            get; set;
//        }

//        public CloudinarySDK()
//        {
//            Init();
//        }

//        public void Init()
//        {
//            Account account = new Account(
//                "socialladder",
//                "912913465365124",
//                "L8C6ce8W52mKXgazvc1Tr96YZjs");

//            cloudinary = new Cloudinary(account);
//        }

//        /// <summary>
//        /// Upload a file by its path.
//        /// </summary>
//        /// <returns>The upload end path.</returns>
//        /// <param name="path">Path.</param>
//        public async Task<string> Upload(string path)
//        {
//            ImageUploadResult uploadResult = null;
//            try
//            {
//                ImageUploadParams uploadParams = new ImageUploadParams()
//                {
//                    File = new FileDescription(path),
//                    PublicId = GetGuid
//                };
//                uploadResult = await cloudinary.UploadAsync(uploadParams);
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine(ex.Message);
//            }
//            return uploadResult != null ? uploadResult.Uri.ToString() : null;
//        }

//        string GetGuid
//        {
//            get
//            {
//                return String.Format("{0}.{1}.jpg", Guid.NewGuid().ToString(), DateTime.Now.ToString("yyyy-MM-dd-HH:mm:ss"));
//                //return String.Format("{0}.{1}.jpg", SL.Profile.UserGUID, DateTime.Now.ToString("yyyy-MM-dd-HH:mm:ss"));
//            }
//        }

//        public async Task<string> Upload(Stream stream)
//        {
//            ImageUploadResult uploadResult = null;
//            try
//            {
//                var uploadParams = new ImageUploadParams()
//                {
//                    File = new FileDescription(GetGuid, stream),
//                    PublicId = GetGuid
//                };
//                uploadResult = await cloudinary.UploadAsync(uploadParams);
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine(ex.Message);
//            }
//            return uploadResult != null ? uploadResult.Uri.ToString() : null;
//        }

//        public async Task<string> Upload(byte[] data)
//        {
//            ImageUploadResult uploadResult = null;
//            try
//            {
//                Stream stream = new MemoryStream(data);
//                var uploadParams = new ImageUploadParams()
//                {
//                    File = new FileDescription(GetGuid, stream),
//                    PublicId = GetGuid
//                };
//                uploadResult = await cloudinary.UploadAsync(uploadParams);
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine(ex.Message);
//            }
//            return uploadResult != null ? uploadResult.Uri.ToString() : null;
//        }

//        public async Task<string> UploadGetId(Stream stream)
//        {
//            ImageUploadResult uploadResult = null;
//            try
//            {
//                var uploadParams = new ImageUploadParams()
//                {
//                    File = new FileDescription(GetGuid, stream),
//                    PublicId = GetGuid
//                };
//                uploadResult = await cloudinary.UploadAsync(uploadParams);
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine(ex.Message);
//            }
//            return uploadResult != null ? uploadResult.PublicId.ToString() : null;
//        }

//        /// <summary>
//        /// Delete the specified url.
//        /// </summary>
//        /// <returns>The delete.</returns>
//        /// <param name="url">URL.</param>
//        public void Delete(string url)
//        {
//            DeletionParams deletionParams = new DeletionParams(url);
//            cloudinary.DestroyAsync(deletionParams);
//        }
//    }
//}

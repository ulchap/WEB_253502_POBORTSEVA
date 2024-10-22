﻿using Microsoft.AspNetCore.Http;

namespace WEB_253502_POBORTSEVA.UI.Services.FileService
{
    public class ApiFileService : IFileService
    {
        private readonly HttpClient _httpClient;

        public ApiFileService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> SaveFileAsync(IFormFile formFile)
        {
            var request = new HttpRequestMessage()
            {
                Method = HttpMethod.Post,
            };

            var extension = Path.GetExtension(formFile.FileName);
            var newName = Path.ChangeExtension(Path.GetRandomFileName(), extension);
            var content = new MultipartFormDataContent();
            var streamContent = new StreamContent(formFile.OpenReadStream());
            content.Add(streamContent, "file", newName);
            request.Content = content;

            var response = await _httpClient.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync();
            }

            return string.Empty;
        }

        public async Task DeleteFileAsync(string fileName)
        {
            var response = await _httpClient.DeleteAsync($"api/Files/{fileName}");
            response.EnsureSuccessStatusCode();
        }
    }

}

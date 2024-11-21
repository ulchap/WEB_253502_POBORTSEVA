namespace WEB_253502_POBORTSEVA.UI.Services.Authentication
{
    public interface ITokenAccessor
    {
        /// <summary> 
        /// Получение access-token 
        /// </summary> 
        /// <returns></returns> 
        Task<string> GetAccessTokenAsync();

        /// <summary> 
        /// Добавление заголовка Authorization : bearer 
        /// </summary> 
        /// <param name="httpClient">HttpClient, в который добавляется заголовок</param> 
        /// <returns></returns> 
        Task SetAuthorizationHeaderAsync(HttpClient httpClient);
    }

}

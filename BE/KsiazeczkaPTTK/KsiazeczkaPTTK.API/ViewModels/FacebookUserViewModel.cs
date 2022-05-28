namespace KsiazeczkaPTTK.API.ViewModels
{
    public class FacebookUserViewModel
    {
        public string AccessToken { get; set; }
        public string Email { get; set; }
        public int ExpiresIn { get; set; }
        public string GraphDomain { get; set; }
        public string Id { get; set; }
        public string Name { get; set; }
        public string SignedRequest { get; set; }
        public string UserID { get; set; }
    }
}

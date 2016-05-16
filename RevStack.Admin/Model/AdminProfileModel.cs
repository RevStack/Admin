using System;

namespace RevStack.Admin
{
    public class AdminProfileModel
    {
        public string Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }

        public AdminProfileModel()
        {
            Id = Guid.NewGuid().ToString();
        }
    }
}
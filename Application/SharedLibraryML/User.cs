using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SharedLibraryML
{
    public class User
    {
        private int _id; 
        private string _username;
        private string _password;
        private string _email;
        private string _phoneNumber;
        private Role role;
        private string _fileImage;
        public bool IsActive { get; set; }

        public User (int id, string username, string password, string email, string phoneNumber, Role role, string fileImage)
        {
            _id = id;
            _username = username;
            _password = password;
            _email = email;
            _phoneNumber = phoneNumber;
            this.role = role;
            _fileImage = fileImage;

           

        }

        public User(string username, string password, string email, string phoneNumber, Role role, string fileImage)
        {
            if (string.IsNullOrWhiteSpace(username))
                throw new ArgumentException("Username is required.");

            if (username.Length < 4 || username.Length > 50)
                throw new ArgumentException("Username must be between 4 and 50 characters.");

            if (string.IsNullOrWhiteSpace(email) ||
                !Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                throw new ArgumentException("Invalid email format.");

            if (string.IsNullOrWhiteSpace(phoneNumber))
                throw new ArgumentException("Phone number is required.");
            if(role == Role.NONE) { throw new ArgumentException("Please select role"); };
           
            _id = 0;
            _username = username;
            _password = password;
            _email = email;
            _phoneNumber = phoneNumber;
            this.role = role;
            _fileImage = fileImage;
        }

        public int GetId()
        {
            return _id;
        }
        public void SetId(int id)
        {
            if (_id > 1)
                throw new InvalidOperationException("ID is already set.");
            _id = id;
        }
        public string GetUsername()
        { return _username; }
        public string GetPassword() { return _password;}
        public string GetEmail() { return _email;}
        public string GetPhoneNumber() { return _phoneNumber;}
        public Role GetRole()
        { return role;}

        public string GetImage()
        {
            return _fileImage;
        }

        public void SetImage(string imageFilePath)
        {
            this._fileImage = imageFilePath;
        }
    }
}

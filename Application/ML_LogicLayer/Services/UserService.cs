using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using BCrypt.Net;
using SharedLibraryML.Interfaces;
using SharedLibraryML;
using ML_LogicLayer.Validators;

namespace ML_LogicLayer.Services
{
    public class UserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public void CreateUser(User user)
        {
            if (IsEmailTaken(user.GetEmail(), user.GetId()))
                throw new ArgumentException("This email is already taken. Please choose another one.");

            string password = user.GetPassword();
            UserValidator.ValidatePasswordStrength(password);


            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);

            var userWithHashedPassword = new User(
                user.GetUsername(),
                hashedPassword,
                user.GetEmail(),
                user.GetPhoneNumber(),
                user.GetRole(),
                null
             );
            _userRepository.CreateUser(userWithHashedPassword);
        }

        public User ReadUser(int userId) 
        {
           return _userRepository.ReadUser(userId);
        }

        public void UpdateUserInfo(User user, string username, string email, string phonenumber, string image) 
        {
           

            var existingUser = _userRepository.ReadUser(user.GetId());



            if (string.IsNullOrEmpty(image))
            {
                user.SetImage(existingUser.GetImage());
            }

            var updatedUser = new User(
             user.GetId(),
             username,
             user.GetPassword(),
             email,
             phonenumber,
             user.GetRole(),
             image
         );

            _userRepository.UpdateUser(updatedUser);


        }

        public void UpdateUserPassword(User user, string newPassword)
        {
            if (IsEmailTaken(user.GetEmail(), user.GetId()))
                throw new ArgumentException("This email is already taken. Please choose another one.");

            if (string.IsNullOrWhiteSpace(newPassword))
                throw new ArgumentException("New password cannot be empty.");

            UserValidator.ValidatePasswordStrength(newPassword);


            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(newPassword);

            var updatedUser = new User(
                user.GetId(),
                user.GetUsername(),
                hashedPassword,
                user.GetEmail(),
                user.GetPhoneNumber(),
                user.GetRole(),
                user.GetImage()
            );
            _userRepository.UpdateUser(updatedUser);
        }

        public List<User> GetAllUsers() 
        {
            return _userRepository.ReadAllUsers();
        }

        public void DeleteUser(int userId) 
        {
            _userRepository.DeleteUser(userId);
        }

        

        public User? FindUserByEmail(string email)
        {
           
          return _userRepository.FindUserByEmail(email);
        }

        public User AuthenticateUser(string email, string password)
        {
            var user = FindUserByEmail(email);
            if (user == null)
            {
                throw new ArgumentNullException("There is no user with this email");
            }

            if (!VerifyPassword(user, password))
            {
                throw new ArgumentException("The password is not correct");
            }


            return user;

        }

        public bool VerifyPassword(User user, string passwordToVerify)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            if (string.IsNullOrWhiteSpace(passwordToVerify))
                return false;

            return BCrypt.Net.BCrypt.Verify(passwordToVerify, user.GetPassword());
        }

        private bool IsEmailTaken(string email, int? excludeUserId = null)
        {
            var existingUser = _userRepository.FindUserByEmail(email);
            if (existingUser == null)
                return false;

            return excludeUserId == null || existingUser.GetId() != excludeUserId.Value;
        }


    }
}

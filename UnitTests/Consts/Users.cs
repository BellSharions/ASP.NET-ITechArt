using Business.DTO;
using Business.Models;
using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests.Consts
{
    public static class Users
    {
        public static User UserTest1 = new()
        {
            Id = 5,
            UserName = "Bell",
            NormalizedUserName = "BELL",
            AdressDelivery = "test",
            Email = "bellsharions@gmail.com",
            EmailConfirmed = true,
            NormalizedEmail = "BELLSHARIONS@GMAIL.COM",
            PhoneNumber = "375293798389",
            PasswordHash = "Ugfkr23dff@FDDFFF"
        };
        public static User UserTest2 = new()
        {
            Id = 5,
            UserName = "Bell",
            NormalizedUserName = "BELL",
            AdressDelivery = "test",
            Email = "bellsharions@gmail.com",
            EmailConfirmed = true,
            NormalizedEmail = "BELLSHARIONS@GMAIL.COM",
            PhoneNumber = "375293798389",
            PasswordHash = "Ugfkr23dff@FDDFFF"
        };
        public static User UserTest3 = new()
        {
            Id = 5,
            UserName = "Bell",
            NormalizedUserName = "BELL",
            AdressDelivery = "test",
            Email = "bellsharions@gmail.com",
            EmailConfirmed = true,
            NormalizedEmail = "BELLSHARIONS@GMAIL.COM",
            PhoneNumber = "375293798389",
            PasswordHash = "Ugfkr23dff@FDDFFF"
        };
        public static User UserTest4 = new()
        {
            Id = 5,
            UserName = "Bell",
            NormalizedUserName = "BELL",
            AdressDelivery = "test",
            Email = "bellsharions@gmail.com",
            EmailConfirmed = true,
            NormalizedEmail = "BELLSHARIONS@GMAIL.COM",
            PhoneNumber = "375293798389",
            PasswordHash = "Ugfkr23dff@FDDFFF"
        };
        public static User UserTest5 = new()
        {
            Id = 5,
            UserName = "Bell",
            NormalizedUserName = "BELL",
            AdressDelivery = "test",
            Email = "bellsharions@gmail.com",
            EmailConfirmed = false,
            NormalizedEmail = "BELLSHARIONS@GMAIL.COM",
            PhoneNumber = "375293798389",
            PasswordHash = "Ugfkr23dff@FDDFFF"
        };
        public static User UserTest6 = new()
        {
            Id = 5,
            UserName = "BelL",
            NormalizedUserName = "BELL",
            AdressDelivery = "tesT",
            Email = "bellsharions@gmail.com",
            EmailConfirmed = false,
            NormalizedEmail = "BELLSHARIONS@GMAIL.COM",
            PhoneNumber = "375293798389",
            PasswordHash = "Ugfkr23dff@FDDFFF"
        };
        public static CreateUserModel UserCreationTest1 = new()
        {
            UserName = "Bell",
            AdressDelivery = "test",
            Email = "bellsharions@gmail.com",
            PhoneNumber = "375293798389",
            Password = "12345678Bb#"
        };
        public static CreateUserModel UserCreationTest2 = new()
        {
            UserName = "Bell",
            AdressDelivery = "test",
            Email = "bellsharions@gmail.com",
            PhoneNumber = "375293798389",
            Password = "13345678Bb#"
        };
        public static CreateUserModel UserCreationTest3 = new()
        {
            UserName = "Bell",
            AdressDelivery = "test",
            Email = "bellsharions@gmailcom",
            PhoneNumber = "375293798389",
            Password = "13345678Bb#"
        };
        public static CreateUserModel UserCreationTest4 = new()
        {
            UserName = "Bell",
            AdressDelivery = "test",
            Email = "bellsharions@gmail.com",
            PhoneNumber = "375293798389",
            Password = "wrong"
        };
        public static ChangeUserInfoDto UserChangeTest1 = new()
        {
            UserName = "BelL",
            AdressDelivery = "tesT",
            Email = "bellsharions@gmail.com",
            PhoneNumber = "375293798389"
        };
        public static ChangePasswordUserDto UserPasswordTest1 = new()
        {
            Id = 5,
            OldPassword = "13345678Bb#",
            NewPassword = "12345678Bb#"
        };
        public static UserInfoDto UserInfoTest1 = new()
        {
            UserName = "Bell",
            AdressDelivery = "test",
            Email = "bellsharions@gmail.com",
            PhoneNumber = "375293798389"
        };
    }
}

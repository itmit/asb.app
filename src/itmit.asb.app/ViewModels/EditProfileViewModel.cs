using System;
using System.Windows.Input;
using itmit.asb.app.Models;
using itmit.asb.app.Services;
using itmit.asb.app.Views;
using Realms;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace itmit.asb.app.ViewModels
{
    class EditProfileViewModel : BaseViewModel
    {
        private string _name;
        private string _passport;
        private string _email;
        private string _organization;
        private string _director;
        private string _inn;
        private string _ogrn;
        private readonly Realm _realm = Realm.GetInstance();

        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }
    
        public string Passport
        {
            get => _passport;
            set => SetProperty(ref _passport, value);
        }

        public string Email
        {
            get => _email;
            set => SetProperty(ref _email, value);
        }

        public string Organization
        {
            get => _organization;
            set => SetProperty(ref _organization, value);
        }

        public string Director 
        { 
            get => _director;
            set => SetProperty(ref _director, value);
        }

        public string INN 
        { 
            get => _inn;
            set => SetProperty(ref _inn, value);
        }

        public string OGRN 
        { 
            get => _ogrn;
            set => SetProperty(ref _ogrn, value);
        }

        public bool IsEntity
            => App.User.UserType == UserType.Entity;

        public bool IsIndividual
            => App.User.UserType == UserType.Individual;

        public EditProfileViewModel()
        {
            var user = App.User;

            _name = user.Name;
            _passport = user.Passport;
            _organization = user.Organization;
            _ogrn = user.Ogrn;
            _email = user.Email;
            _director = user.Director;
            _inn = user.Inn;

            Sender = new RelayCommand(obj =>
            {
                    _realm.Write(() =>
                    {
                        user.Name = Name;
                        user.Passport = Passport;
                        user.Email = Email;
                        user.Email = Email;
                        user.Organization = Organization;
                        user.Director = Director;
                        user.Inn = INN;
                        user.Ogrn = OGRN;
                    });
                SaveButton(user);
            },
                obj => !IsBusy && Connectivity.NetworkAccess == NetworkAccess.Internet);
        }

        public ICommand Sender 
        { 
            get; 
        }
        
        private async void SaveButton(User user)
        {
            var app = Application.Current as App;
            if (app==null)
            {
                return;
            }
            var service = new AuthService();
            bool res = await service.SetEditProfile(user);
            if (res)
            {
                await Application.Current.MainPage.DisplayAlert("Уведомление", "Данные успешно сохранены", "ОK");
            }
        }
    }
}

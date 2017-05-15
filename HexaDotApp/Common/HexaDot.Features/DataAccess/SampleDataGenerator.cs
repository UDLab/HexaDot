using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using HexaDot.Data.Models;
using HexaDot.Core.Settings;
//using TaskStatus = HexaDot.Data.Models.TaskStatus;

namespace HexaDot.Features.DataAccess
{
    public class SampleDataGenerator
    {
        private readonly HexaDotContext _context;
        private readonly SampleDataSettings _settings;
        private readonly GeneralSettings _generalSettings;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly TimeZoneInfo _timeZone = TimeZoneInfo.Local;

        public SampleDataGenerator(HexaDotContext context, IOptions<SampleDataSettings> options, IOptions<GeneralSettings> generalSettings, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _settings = options.Value;
            _generalSettings = generalSettings.Value;
            _userManager = userManager;
        }

        public void InsertTestData()
        {
            // Avoid polluting the database if there's already something in there.
            if (_context.Institutes.Any())
            {
                return;
            }



            var institutes = new List<Institute>();
            var users = new List<ApplicationUser>();

            #region Institute

            var institute = new Institute
            {
                Name = "Uppsala Design Lab",
                LogoUrl = "http://www.udlab.org/udlab-logo.png",
                WebUrl = "http://www.udlab.org",
            };

            #endregion



            #region Insert into DB

            _context.Institutes.Add(institute);

            #endregion


            #region Users for Events
            var username1 = $"{_settings.DefaultUsername}1.com";
            var username2 = $"{_settings.DefaultUsername}2.com";
            var username3 = $"{_settings.DefaultUsername}3.com";

            var user1 = new ApplicationUser
            {
                FirstName = "FirstName1",
                LastName = "LastName1",
                UserName = username1,
                Email = username1,
                EmailConfirmed = true,
                //TimeZoneId = _timeZone.Id,
                PhoneNumber = "111-111-1111"
            };
            _userManager.CreateAsync(user1, _settings.DefaultAdminPassword).GetAwaiter().GetResult();
            users.Add(user1);

            var user2 = new ApplicationUser
            {
                FirstName = "FirstName2",
                LastName = "LastName2",
                UserName = username2,
                Email = username2,
                EmailConfirmed = true,
                //TimeZoneId = _timeZone.Id,
                PhoneNumber = "222-222-2222"
            };
            _userManager.CreateAsync(user2, _settings.DefaultAdminPassword).GetAwaiter().GetResult();
            users.Add(user2);

            var user3 = new ApplicationUser
            {
                FirstName = "FirstName3",
                LastName = "LastName3",
                UserName = username3,
                Email = username3,
                EmailConfirmed = true,
                //TimeZoneId = _timeZone.Id,
                PhoneNumber = "333-333-3333"
            };
            _userManager.CreateAsync(user3, _settings.DefaultAdminPassword).GetAwaiter().GetResult();
            users.Add(user3);
            #endregion






            #region Wrap Up DB
            _context.SaveChanges();
            #endregion
        }

        private DateTimeOffset AdjustToTimezone(DateTimeOffset dateTimeOffset, TimeZoneInfo timeZone)
        {
            return new DateTimeOffset(dateTimeOffset.DateTime, timeZone.GetUtcOffset(dateTimeOffset.DateTime));
        }



        #region Sample Data Helper methods
        private static T GetRandom<T>(List<T> list)
        {
            var rand = new Random();
            return list[rand.Next(list.Count)];
        }


        #endregion

        /// <summary>
        /// Creates a administrative user who can manage the inventory.
        /// </summary>
        public async Task CreateAdminUser()
        {
            var user = await _userManager.FindByNameAsync(_settings.DefaultAdminUsername);
            if (user == null)
            {
                user = new ApplicationUser
                {
                    FirstName = "FirstName4",
                    LastName = "LastName4",
                    UserName = _settings.DefaultAdminUsername,
                    Email = _settings.DefaultAdminUsername,
                    //TimeZoneId = _timeZone.Id,
                    EmailConfirmed = true,
                    PhoneNumber = "444-444-4444"
                };
                _userManager.CreateAsync(user, _settings.DefaultAdminPassword).GetAwaiter().GetResult();
                _userManager.AddClaimAsync(user, new Claim(Security.ClaimTypes.UserType, "InsCoordinator")).GetAwaiter().GetResult();

                var user2 = new ApplicationUser
                {
                    FirstName = "FirstName5",
                    LastName = "LastName5",
                    UserName = _settings.DefaultInstituteUsername,
                    Email = _settings.DefaultInstituteUsername,
                    //TimeZoneId = _timeZone.Id,
                    EmailConfirmed = true,
                    PhoneNumber = "555-555-5555"
                };
                // For the sake of being able to exercise Institute-specific stuff, we need to associate a organization.
                await _userManager.CreateAsync(user2, _settings.DefaultAdminPassword);
                await _userManager.AddClaimAsync(user2, new Claim(Security.ClaimTypes.UserType, "SysAdmin"));
                await _userManager.AddClaimAsync(user2, new Claim(Security.ClaimTypes.Institute, _context.Institutes.First().Id.ToString()));

                var user3 = new ApplicationUser
                {
                    FirstName = "FirstName5",
                    LastName = "LastName5",
                    UserName = _settings.DefaultUsername,
                    Email = _settings.DefaultUsername,
                    //TimeZoneId = _timeZone.Id,
                    EmailConfirmed = true,
                    PhoneNumber = "666-666-6666"
                };
                await _userManager.CreateAsync(user3, _settings.DefaultAdminPassword);
            }
        }
    }
}

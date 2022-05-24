using KsiazeczkaPttk.Domain.Models;
using KsiazeczkaPTTK.DAL.InputModels;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace KsiazeczkaPttk.DAL
{
    public static class TouristsBookSeed
    {

        public static async Task Seed(TouristsBookContext context)
        {
            if (await context.MountainGroups.AnyAsync())
            {
                return;
            }

            await SeedFromCode(context);
            await SeedFromFiles(context);
        }

        private static async Task SeedFromCode(TouristsBookContext context)
        {
            var roleUzytkownikow = new List<UserRole>()
            {
                new UserRole(){ Name = "Administrator"},
                new UserRole(){ Name = "Turysta"},
                new UserRole(){ Name = "Przodownik"},
                new UserRole(){ Name = "Pracownik"},
            };

            var uzytkownicy = new List<User>()
            {
                new User {Login = "Turysta1", Password = "Pa55word", UserRoleName = roleUzytkownikow[1].Name, UserRole = roleUzytkownikow[1], FirstName = "Johny", LastName = "Rambo", Email = "johny.rambo@gmail.com"},
                new User {Login = "Przodownik1", Password = "Pa55word", UserRoleName = roleUzytkownikow[2].Name, UserRole = roleUzytkownikow[2], FirstName = "Henry", LastName = "Walton", Email = "henry.Walton@gmail.com"},
                new User {Login = "Pracownik1", Password = "Pa55word", UserRoleName = roleUzytkownikow[3].Name, UserRole = roleUzytkownikow[3], FirstName = "Rocky", LastName = "Balboa", Email = "rocky.balboa@gmail.com"},
            };

            var ksiazeczki = new List<TouristsBook>()
            {
                new TouristsBook {OwnerId = uzytkownicy[0].Login, Points = 5, Owner = uzytkownicy[0], Disability = false}
            };

            var grupyGorskie = new List<MountainGroup>()
            {
                new MountainGroup {Id = 1, Name = "Tatry i Podtatrze" },
                new MountainGroup {Id = 2, Name = "Beskidy Zachodnie" },
                new MountainGroup {Id = 3, Name = "Beskidy Wschodnie" },
                new MountainGroup {Id = 4, Name = "Góry Świętokrzyskie" },
                new MountainGroup {Id = 5, Name = "Sudety" },
            };

            var pasmaGorskie = new List<MountainRange>()
            {
                new MountainRange(){ Id = 1, Name = "Tatry Wysokie", GroupId = grupyGorskie[0].Id, MountainGroup = grupyGorskie[0] },
                new MountainRange(){ Id = 2, Name = "Tatry Zachodnie", GroupId = grupyGorskie[0].Id, MountainGroup = grupyGorskie[0] },
                new MountainRange(){ Id = 3, Name = "Podtatrze", GroupId = grupyGorskie[0].Id, MountainGroup = grupyGorskie[0] },

                new MountainRange(){ Id = 4, Name = "Beskid Śląski", GroupId = grupyGorskie[1].Id, MountainGroup = grupyGorskie[1] },
                new MountainRange(){ Id = 5, Name = "Beskid Żywiecki", GroupId = grupyGorskie[1].Id, MountainGroup = grupyGorskie[1] },

                new MountainRange(){ Id = 6, Name = "Bieszczady", GroupId = grupyGorskie[2].Id, MountainGroup = grupyGorskie[2] },

                new MountainRange(){ Id = 7, Name = "Góry Izerskie", GroupId = grupyGorskie[4].Id, MountainGroup = grupyGorskie[4] },
                new MountainRange(){ Id = 8, Name = "Karkonosze", GroupId = grupyGorskie[4].Id, MountainGroup = grupyGorskie[4] },
                new MountainRange(){ Id = 9, Name = "Góry Kaczawskie", GroupId = grupyGorskie[4].Id, MountainGroup = grupyGorskie[4] },
            };

            var punktyTerenowePubliczne = new List<TerrainPoint>
            {
                new TerrainPoint { Id = 1, Name = "Rusinowa Polana", Lat = 49.2608, Lng = 20.0895, Mnpm = 1200},
                new TerrainPoint { Id = 2, Name = "Łysa Polana", Lat = 49.2706, Lng = 20.1179, Mnpm = 971 },
                new TerrainPoint { Id = 3, Name = "Gęsia Szyja", Lat = 49.2597, Lng = 20.0778, Mnpm = 1489 },
                new TerrainPoint { Id = 4, Name = "Rówień Waksmundzka", Lat = 49.2553, Lng = 20.0655, Mnpm = 1440 },
                new TerrainPoint { Id = 5, Name = "Psia Trawka", Lat = 49.2696, Lng = 20.0367, Mnpm = 1185 },
                new TerrainPoint { Id = 6, Name = "Schronisko PTTK nad Morskim Okiem", Lat = 49.2013, Lng = 20.0713, Mnpm = 1410 },
                new TerrainPoint { Id = 7, Name = "Czarny Staw nad Morskim Okiem", Lat = 49.1883, Lng = 20.0754, Mnpm = 1580 },
                new TerrainPoint { Id = 8, Name = "Dolina za Mnichem", Lat = 49.2434, Lng = 20.0072, Mnpm = 1780 },
                new TerrainPoint { Id = 9, Name = "Szpiglasowa Przełęcz", Lat = 49.2434, Lng = 20.0072, Mnpm = 2110 },
                new TerrainPoint { Id = 10, Name = "Schronisko PTTK na Hali Gąsienicowej", Lat = 49.2434, Lng = 20.0072, Mnpm = 1500 },
            };

            var punktyTerenowePrywatne = new List<TerrainPoint>
            {
                new TerrainPoint { Id = 101, Name = "Dolina Pańczyca", Lat = 49.2414, Lng = 20.0172, Mnpm = 1628, TouristsBookOwner = ksiazeczki[0].OwnerId, TouristsBook = ksiazeczki[0] },
            };

            var odcinkiPubliczne = new List<Segment>
            {
                new Segment { Id = 1, Version = 1, Name = "Odcinek Tatry Wysokie 1", IsActive = true, MountainRangeId = pasmaGorskie[0].Id, MountainRange = pasmaGorskie[0], Points = 4, PointsBack = 1, FromId = punktyTerenowePubliczne[0].Id, From = punktyTerenowePubliczne[0], TargetId = punktyTerenowePubliczne[2].Id, Target = punktyTerenowePubliczne[2] },
                new Segment { Id = 2, Version = 1, Name = "Odcinek Tatry Wysokie 2", IsActive = true, Points = 2, PointsBack = 1, FromId = punktyTerenowePubliczne[3].Id, From = punktyTerenowePubliczne[3], TargetId = punktyTerenowePubliczne[2].Id, Target = punktyTerenowePubliczne[2], MountainRangeId = pasmaGorskie[0].Id, MountainRange = pasmaGorskie[0] },
                new Segment { Id = 3, Version = 1, Name = "Odcinek Tatry Wysokie 3", IsActive = true, Points = 5, PointsBack = 3, FromId = punktyTerenowePubliczne[4].Id, From = punktyTerenowePubliczne[4], TargetId = punktyTerenowePubliczne[3].Id, Target = punktyTerenowePubliczne[3], MountainRangeId = pasmaGorskie[0].Id, MountainRange = pasmaGorskie[0] },
                new Segment { Id = 4, Version = 1, Name = "Odcinek Tatry Wysokie 4", IsActive = true, Points = 4, PointsBack = 7, FromId = punktyTerenowePubliczne[9].Id, From = punktyTerenowePubliczne[9], TargetId = punktyTerenowePubliczne[4].Id, Target = punktyTerenowePubliczne[4], MountainRangeId = pasmaGorskie[0].Id, MountainRange = pasmaGorskie[0] },
                new Segment { Id = 5, Version = 1, Name = "Odcinek Tatry Wysokie 5", IsActive = true, Points = 4, PointsBack = 2, FromId = punktyTerenowePubliczne[5].Id, From = punktyTerenowePubliczne[5], TargetId = punktyTerenowePubliczne[6].Id, Target = punktyTerenowePubliczne[6], MountainRangeId = pasmaGorskie[0].Id, MountainRange = pasmaGorskie[0] },
            };

            var odcinkiPrywatne = new List<Segment>
            {
                new Segment { Id = 101, Version = 1, Name = "Odcinek prywatny 1", IsActive = true, Points = 2, PointsBack = 3, FromId = punktyTerenowePubliczne[9].Id, From = punktyTerenowePubliczne[9], TargetId = punktyTerenowePrywatne[0].Id, Target = punktyTerenowePrywatne[0], MountainRangeId = pasmaGorskie[0].Id, MountainRange = pasmaGorskie[0], TouristsBookOwner = ksiazeczki[0].OwnerId, TouristsBook = ksiazeczki[0] },
            };

            var wycieczki = new List<Trip>
            {
                new Trip { Id = 1, Name = "Wycieczka 1",  TouristsBookId = ksiazeczki[0].OwnerId, TouristsBook = ksiazeczki[0], Status = Domain.Enums.TripStatus.Werified },
                new Trip { Id = 2, Name = "Wycieczka 2", TouristsBookId = ksiazeczki[0].OwnerId, TouristsBook = ksiazeczki[0], Status = Domain.Enums.TripStatus.Planned }
            };

            var przebyteOdcinki = new List<SegmentTravel>
            {
                new SegmentTravel { Id = 1, Order = 1, SegmentId = odcinkiPubliczne[0].Id, Segment = odcinkiPubliczne[0], IsBack = false, TripId = wycieczki[0].Id, Trip = wycieczki[0] },
                new SegmentTravel { Id = 2, Order = 2, SegmentId = odcinkiPubliczne[1].Id, Segment = odcinkiPubliczne[1], IsBack = true, TripId = wycieczki[0].Id, Trip = wycieczki[0] },
                new SegmentTravel { Id = 3, Order = 3, SegmentId = odcinkiPubliczne[2].Id, Segment = odcinkiPubliczne[2], IsBack = true, TripId = wycieczki[0].Id, Trip = wycieczki[0] },
                new SegmentTravel { Id = 4, Order = 4, SegmentId = odcinkiPubliczne[3].Id, Segment = odcinkiPubliczne[3], IsBack = true, TripId = wycieczki[0].Id, Trip = wycieczki[0] },
                new SegmentTravel { Id = 5, Order = 5, SegmentId = odcinkiPrywatne[0].Id, Segment = odcinkiPrywatne[0], IsBack = false, TripId = wycieczki[0].Id, Trip = wycieczki[0] },
            };

            var potwierdzeniaAdministracyjne = new List<Confirmation>
            {
                new Confirmation { Id = 1, Url = "RusinowaPolanaUrl", TerrainPointId = punktyTerenowePubliczne[0].Id, TerrainPoint = punktyTerenowePubliczne[0], IsAdministration = true, Type = Domain.Enums.ConfirmationType.QrCode },
                new Confirmation { Id = 2, Url = "ŁysaPolanaUrl", TerrainPointId = punktyTerenowePubliczne[1].Id, TerrainPoint = punktyTerenowePubliczne[1], IsAdministration = true, Type = Domain.Enums.ConfirmationType.QrCode },
                new Confirmation { Id = 3, Url = "GęsiaSzyjaUrl", TerrainPointId = punktyTerenowePubliczne[2].Id, TerrainPoint = punktyTerenowePubliczne[2], IsAdministration = true, Type = Domain.Enums.ConfirmationType.QrCode },
                new Confirmation { Id = 4, Url = "RówieńWaksmundzkaUrl", TerrainPointId = punktyTerenowePubliczne[3].Id, TerrainPoint = punktyTerenowePubliczne[3], IsAdministration = true, Type = Domain.Enums.ConfirmationType.QrCode },
                new Confirmation { Id = 5, Url = "PsiaTrawkaUrl", TerrainPointId = punktyTerenowePubliczne[4].Id, TerrainPoint = punktyTerenowePubliczne[4], IsAdministration = true, Type = Domain.Enums.ConfirmationType.QrCode },
                new Confirmation { Id = 6, Url = "SchroniskoPTTKnadMorskimOkiemUrl", TerrainPointId = punktyTerenowePubliczne[5].Id, TerrainPoint = punktyTerenowePubliczne[5], IsAdministration = true, Type = Domain.Enums.ConfirmationType.QrCode },
                new Confirmation { Id = 7, Url = "CzarnyStawnadMorskimOkiemUrl", TerrainPointId = punktyTerenowePubliczne[6].Id, TerrainPoint = punktyTerenowePubliczne[6], IsAdministration = true, Type = Domain.Enums.ConfirmationType.QrCode },
                new Confirmation { Id = 8, Url = "DolinazaMnichemUrl", TerrainPointId = punktyTerenowePubliczne[7].Id, TerrainPoint = punktyTerenowePubliczne[7], IsAdministration = true, Type = Domain.Enums.ConfirmationType.QrCode },
                new Confirmation { Id = 9, Url = "SzpiglasowaPrzełęczUrl", TerrainPointId = punktyTerenowePubliczne[8].Id, TerrainPoint = punktyTerenowePubliczne[8], IsAdministration = true, Type = Domain.Enums.ConfirmationType.QrCode },
                new Confirmation { Id = 10, Url = "SchroniskoPTTKnaHaliGąsienicowejUrl", TerrainPointId = punktyTerenowePubliczne[9].Id, TerrainPoint = punktyTerenowePubliczne[9], IsAdministration = true, Type = Domain.Enums.ConfirmationType.QrCode },
            };

            var potwierdzeniaPrywatne = new List<Confirmation>
            {
                new Confirmation { Id = 101, Url = "b95fc8cd-a474-44f2-968a-6da2118c1f6e_rusinowaPolana.jpg", Date = new System.DateTime(2021, 7, 21, 12, 10, 0), TerrainPointId = punktyTerenowePubliczne[0].Id, TerrainPoint = punktyTerenowePubliczne[0], IsAdministration = false, Type = Domain.Enums.ConfirmationType.Image },
                new Confirmation { Id = 102, Url = "770f1809-59d5-46ba-9777-7095457a5884_dolinaPanczyca.jpg", Date = new System.DateTime(2021, 7, 21, 14, 10, 0), TerrainPointId = punktyTerenowePrywatne[0].Id, TerrainPoint = punktyTerenowePrywatne[0], IsAdministration = false, Type = Domain.Enums.ConfirmationType.Image },
                new Confirmation { Id = 103, Url = "6d945f8f-19ae-4d63-bd2c-2f3b834d36f6_gesiaSzyja.jpg", Date = new System.DateTime(2021, 7, 21, 15, 50, 0), TerrainPointId = punktyTerenowePubliczne[2].Id, TerrainPoint = punktyTerenowePubliczne[2], IsAdministration = false, Type = Domain.Enums.ConfirmationType.Image },
                new Confirmation { Id = 104, Url = "cd467e9c-a530-45b7-a0f2-4595fad24bfd_rowienWaksmundzka.jpg", Date = new System.DateTime(2021, 7, 21, 17, 10, 0), TerrainPointId = punktyTerenowePubliczne[3].Id, TerrainPoint = punktyTerenowePubliczne[3], IsAdministration = false, Type = Domain.Enums.ConfirmationType.Image },
                new Confirmation { Id = 105, Url = "263f37d7-38d3-4747-9335-1812f5fb3a90_psiaTrawka.jpg", Date = new System.DateTime(2021, 7, 21, 17, 50, 0), TerrainPointId = punktyTerenowePubliczne[4].Id, TerrainPoint = punktyTerenowePubliczne[4], IsAdministration = false, Type = Domain.Enums.ConfirmationType.Image },
                new Confirmation { Id = 106, Url = "75521f1b-40f9-4ce3-a0ab-8ab5baf282e8_schroniskoHalaGasienicowa.jfif", Date = new System.DateTime(2021, 7, 21, 18, 45, 0), TerrainPointId = punktyTerenowePubliczne[9].Id, TerrainPoint = punktyTerenowePubliczne[9], IsAdministration = false, Type = Domain.Enums.ConfirmationType.Image },
            };

            var potwierdzeniaOdcinkow = new List<SegmentConfirmation>
            {
                new SegmentConfirmation { Id = 1, SegmentTravelId = przebyteOdcinki[0].Id, SegmentTravel = przebyteOdcinki[0], ConfirmationId = potwierdzeniaPrywatne[0].Id, Confirmation = potwierdzeniaPrywatne[0] },
                new SegmentConfirmation { Id = 2, SegmentTravelId = przebyteOdcinki[0].Id, SegmentTravel = przebyteOdcinki[0], ConfirmationId = potwierdzeniaPrywatne[2].Id, Confirmation = potwierdzeniaPrywatne[2] },
                new SegmentConfirmation { Id = 3, SegmentTravelId = przebyteOdcinki[1].Id, SegmentTravel = przebyteOdcinki[1], ConfirmationId = potwierdzeniaPrywatne[2].Id, Confirmation = potwierdzeniaPrywatne[2] },
                new SegmentConfirmation { Id = 4, SegmentTravelId = przebyteOdcinki[1].Id, SegmentTravel = przebyteOdcinki[1], ConfirmationId = potwierdzeniaPrywatne[3].Id, Confirmation = potwierdzeniaPrywatne[3] },
                new SegmentConfirmation { Id = 5, SegmentTravelId = przebyteOdcinki[2].Id, SegmentTravel = przebyteOdcinki[2], ConfirmationId = potwierdzeniaPrywatne[3].Id, Confirmation = potwierdzeniaPrywatne[3] },
                new SegmentConfirmation { Id = 6, SegmentTravelId = przebyteOdcinki[2].Id, SegmentTravel = przebyteOdcinki[2], ConfirmationId = potwierdzeniaPrywatne[4].Id, Confirmation = potwierdzeniaPrywatne[4] },
                new SegmentConfirmation { Id = 7, SegmentTravelId = przebyteOdcinki[3].Id, SegmentTravel = przebyteOdcinki[3], ConfirmationId = potwierdzeniaPrywatne[4].Id, Confirmation = potwierdzeniaPrywatne[4] },
                new SegmentConfirmation { Id = 8, SegmentTravelId = przebyteOdcinki[3].Id, SegmentTravel = przebyteOdcinki[3], ConfirmationId = potwierdzeniaPrywatne[5].Id, Confirmation = potwierdzeniaPrywatne[5] },
                new SegmentConfirmation { Id = 9, SegmentTravelId = przebyteOdcinki[4].Id, SegmentTravel = przebyteOdcinki[4], ConfirmationId = potwierdzeniaPrywatne[5].Id, Confirmation = potwierdzeniaPrywatne[5] },
                new SegmentConfirmation { Id = 10, SegmentTravelId = przebyteOdcinki[4].Id, SegmentTravel = przebyteOdcinki[4], ConfirmationId = potwierdzeniaPrywatne[1].Id, Confirmation = potwierdzeniaPrywatne[1] },
            };

            await context.UserRoles.AddRangeAsync(roleUzytkownikow);
            await context.SaveChangesAsync();
            await context.Users.AddRangeAsync(uzytkownicy);
            await context.SaveChangesAsync();
            await context.MountainGroups.AddRangeAsync(grupyGorskie);
            await context.SaveChangesAsync();
            await context.MountainRanges.AddRangeAsync(pasmaGorskie);
            await context.SaveChangesAsync();
            await context.TerrainPoints.AddRangeAsync(punktyTerenowePubliczne);
            await context.SaveChangesAsync();
            await context.TerrainPoints.AddRangeAsync(punktyTerenowePrywatne);
            await context.SaveChangesAsync();
            await context.Segments.AddRangeAsync(odcinkiPubliczne);
            await context.SaveChangesAsync();
            await context.Segments.AddRangeAsync(odcinkiPrywatne);
            await context.SaveChangesAsync();
            await context.Trips.AddRangeAsync(wycieczki);
            await context.SaveChangesAsync();
            await context.SegmentTravels.AddRangeAsync(przebyteOdcinki);
            await context.SaveChangesAsync();
            await context.Confirmations.AddRangeAsync(potwierdzeniaAdministracyjne);
            await context.SaveChangesAsync();
            await context.Confirmations.AddRangeAsync(potwierdzeniaPrywatne);
            await context.SaveChangesAsync();
            await context.SegmentConfirmations.AddRangeAsync(potwierdzeniaOdcinkow);
            await context.SaveChangesAsync();
        }

        private static async Task SeedFromFiles(TouristsBookContext context)
        {
            var root = Directory.GetParent(Directory.GetCurrentDirectory()).FullName;
            var seedFolder = Path.Combine(root, "KsiazeczkaPTTK.DAL", "Seed_Input");
            if (Directory.Exists(seedFolder))
            {
                var serializationOptions = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };
                foreach (var fileName in Directory.EnumerateFiles(seedFolder, "*.json", SearchOption.AllDirectories))
                {
                    var fileContent = File.ReadAllText(fileName);
                    try
                    {
                        var x = JsonSerializer.Deserialize<RangeSegmentsInput>(fileContent, serializationOptions);
                    }
                    catch (Exception)
                    {
                        Console.WriteLine($"Cannot deserialize {fileName}");
                    }
                }
            }
        }
    }
}

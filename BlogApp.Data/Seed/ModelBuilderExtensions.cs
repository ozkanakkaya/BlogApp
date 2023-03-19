using BlogApp.Core.Entities.Concrete;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BlogApp.Data.Seed
{
    public static class ModelBuilderExtensions
    {
        public static void SeedData(this ModelBuilder builder)
        {
            var superAdminUser = new User
            {
                Id = 1,
                Firstname = "Özkan",
                Lastname = "Akkaya",
                Username = "ozkky",
                PhoneNumber = "5555555555",
                Email = "ozkky@gmail.com",
                ImageUrl = "userImages/defaultUser.png",
                About = "Blog sitesinin süper admini",
                GitHubLink = "ozkanakkaya",
                WebsiteLink = "ozkanakkaya.com",
                GenderId = 1,
                CreatedDate = DateTime.Now,
                UpdatedDate = DateTime.Now,
                CreatedByUsername = "ozkky",
                UpdatedByUsername = "ozkky",
                IsActive = true,
                IsDeleted = false
            };
            superAdminUser.PasswordHash = CreatePasswordHash(superAdminUser, "superadminuser");
            builder.Entity<User>().HasData(superAdminUser);

            var adminUser = new User
            {
                Id = 2,
                Firstname = "Admin",
                Lastname = "Admin",
                Username = "adminuser",
                PhoneNumber = "5555555555",
                Email = "adminuser@gmail.com",
                ImageUrl = "userImages/defaultUser.png",
                About = "Blog sitesinin admini",
                GitHubLink = "adminuser",
                WebsiteLink = "adminuser.com",
                GenderId = 2,
                CreatedDate = DateTime.Now,
                UpdatedDate = DateTime.Now,
                CreatedByUsername = "adminuser",
                UpdatedByUsername = "adminuser",
                IsActive = true,
                IsDeleted = false
            };
            adminUser.PasswordHash = CreatePasswordHash(adminUser, "adminuser");
            builder.Entity<User>().HasData(adminUser);

            var memberUser = new User
            {
                Id = 3,
                Firstname = "Member",
                Lastname = "Member",
                Username = "memberuser",
                PhoneNumber = "5555555555",
                Email = "memberuser@gmail.com",
                ImageUrl = "userImages/defaultUser.png",
                About = "Blog sitesinde üye kullanıcı",
                GitHubLink = "memberuser",
                WebsiteLink = "memberuser.com",
                GenderId = 1,
                CreatedDate = DateTime.Now,
                UpdatedDate = DateTime.Now,
                CreatedByUsername = "editoruser",
                UpdatedByUsername = "editoruser",
                IsActive = true,
                IsDeleted = false
            };
            memberUser.PasswordHash = CreatePasswordHash(memberUser, "memberuser");
            builder.Entity<User>().HasData(memberUser);

            var editorUser = new User
            {
                Id = 4,
                Firstname = "Editor",
                Lastname = "Editor",
                Username = "editoruser",
                PhoneNumber = "5555555555",
                Email = "editoruser@gmail.com",
                ImageUrl = "userImages/defaultUser.png",
                About = "Blog sitesinde üye editör",
                GitHubLink = "editoruser",
                WebsiteLink = "editoruser.com",
                GenderId = 1,
                CreatedDate = DateTime.Now,
                UpdatedDate = DateTime.Now,
                CreatedByUsername = "editoruser",
                UpdatedByUsername = "editoruser",
                IsActive = true,
                IsDeleted = false
            };
            editorUser.PasswordHash = CreatePasswordHash(editorUser, "editoruser");
            builder.Entity<User>().HasData(editorUser);


            builder.Entity<Role>().HasData(new Role[]
            {
                new() { Id = 1, Name = "Category.Create" },
                new() { Id = 2, Name = "Category.Read" },
                new() { Id = 3, Name = "Category.Update" },
                new() { Id = 4, Name = "Category.Delete" },
                new() { Id = 5, Name = "Blog.Create" },
                new() { Id = 6, Name = "Blog.Read" },
                new() { Id = 7, Name = "Blog.Update" },
                new() { Id = 8, Name = "Blog.Delete" },
                new() { Id = 9, Name = "User.Create" },
                new() { Id = 10, Name = "User.Read" },
                new() { Id = 11, Name = "User.Update" },
                new() { Id = 12, Name = "User.Delete" },
                new() { Id = 13, Name = "Role.Create" },
                new() { Id = 14, Name = "Role.Read" },
                new() { Id = 15, Name = "Role.Update" },
                new() { Id = 16, Name = "Role.Delete" },
                new() { Id = 17, Name = "Comment.Create" },
                new() { Id = 18, Name = "Comment.Read" },
                new() { Id = 19, Name = "Comment.Update" },
                new() { Id = 20, Name = "Comment.Delete" },
                new() { Id = 21, Name = "Member" },
                new() { Id = 22, Name = "Admin" },
                new() { Id = 23, Name = "SuperAdmin" }
            });

            builder.Entity<UserRole>().HasData(new UserRole[]
            {
                new(){ Id = 1 , UserId = 1, RoleId = 23 },
                new(){ Id = 2 , UserId = 2, RoleId = 22 },
                new(){ Id = 3 , UserId = 3, RoleId = 21 },
                new(){ Id = 4 , UserId = 3, RoleId = 6 },
                new(){ Id = 5 , UserId = 3, RoleId = 17 },
                new(){ Id = 6 , UserId = 4, RoleId = 21 },
                new(){ Id = 7 , UserId = 4, RoleId = 6 },
                new(){ Id = 8 , UserId = 4, RoleId = 7 },
                new(){ Id = 9 , UserId = 4, RoleId = 17 }

            });

            builder.Entity<Blog>().HasData(new Blog[]
            {
                new()
                {
                    Id = 1 ,
                    Title = "Veri Bilimi ve Makine Öğrenmesi: Geleceğin Teknolojisi",
                    Content = "Son yıllarda teknoloji hızla gelişiyor ve bu gelişmelerle birlikte veri bilimi ve makine öğrenmesi alanları da hızla büyüyor. Ancak, bu terimlerin ne anlama geldiği konusunda hala birçok insan kafa karışıklığı yaşayabilir. Bu yazıda, veri bilimi ve makine öğrenmesi hakkında genel bir bilgi vereceğim ve bunların neden önemli olduğunu anlatacağım.\r\n\r\nVeri bilimi, verilerin analiz edilmesi, yorumlanması ve anlamlı sonuçlar elde edilmesi için istatistik, matematik ve bilgisayar bilimlerinin kullanıldığı bir alandır. Veri bilimciler, verileri toplar, temizler, işler ve model oluştururlar. Bu modeller, öngörülebilir sonuçlar elde etmek için kullanılabilir.\r\n\r\nMakine öğrenmesi ise, bilgisayarların belirli bir görevi yerine getirmek için verileri kullanarak kendi kendine öğrenmesi ve geliştirmesi anlamına gelir. Bu yöntem, birçok alanda kullanılabilir. Örneğin, e-posta spam filtrelemesi, resim tanıma, doğal dil işleme ve daha birçok şey.\r\n\r\nNeden Veri Bilimi ve Makine Öğrenmesi Önemlidir?\r\n\r\nVeri bilimi ve makine öğrenmesi, işletmeler, hükümetler ve toplumlar için önemli bir rol oynamaktadır. Bu alanlar, daha iyi kararlar almak ve verimliliklerini artırmak için büyük miktarda veriyi analiz etmek zorundadır. Veri bilimi ve makine öğrenmesi, bu verilerin anlamlı hale getirilmesine yardımcı olabilir.\r\n\r\nÖrneğin, bir perakende şirketi, müşteri davranışları hakkında daha iyi bir anlayışa sahip olduğunda, daha iyi bir müşteri deneyimi sunabilir ve daha etkili bir pazarlama stratejisi oluşturabilir. Bir sağlık hizmetleri şirketi, büyük veri analizi kullanarak hastalıkların yayılmasını tahmin edebilir ve sağlık hizmetlerinin daha iyi bir şekilde yönetilmesine yardımcı olabilir.\r\n\r\nSonuç olarak, veri bilimi ve makine öğrenmesi, birçok alanda büyük önem taşıyan güçlü araçlardır. Bu alanların hızla gelişmesi, daha iyi kararlar almak ve verimliliği artırmak için daha fazla veriyi analiz etmemizi sağlayacaktır.",
                    ImageUrl = "postImages/defaultImage.png",
                    ViewCount = 100,
                    CommentCount = 0,
                    LikeCount = 100,
                    UserId = 1,
                    CreatedDate = DateTime.Now,
                    UpdatedDate = DateTime.Now,
                    CreatedByUsername = "ozkky",
                    UpdatedByUsername = "ozkky",
                    IsActive = true,
                    IsDeleted = false
                },
                new()
                {
                    Id = 2 ,
                    Title = "Siber Güvenlik Tehditleri ve Önleme Yöntemleri",
                    Content = "Günümüzde teknolojinin hızlı gelişimiyle birlikte siber güvenlik tehditleri de artış göstermiştir. Siber suçlular, bireysel ve kurumsal kullanıcıların bilgi güvenliğini tehdit ederek, zararlı yazılımlar, kimlik avı saldırıları, DDoS saldırıları ve daha birçok yöntemle hedeflerine ulaşmaya çalışmaktadırlar. Bu nedenle siber güvenlik, günümüzde hayati bir öneme sahip bir konudur.\r\n\r\nSiber güvenlik tehditleri, bireysel kullanıcıların yanı sıra, işletmeler ve kurumlar için de büyük bir risk oluşturur. Siber saldırılara maruz kalan şirketler, müşteri güvenini kaybedebilir, maddi zararlarla karşılaşabilir ve hatta itibar kaybına uğrayabilirler. Bu nedenle, kurumlar siber güvenliği ciddiye almalı ve koruma önlemleri almalıdırlar.\r\n\r\nSiber güvenliği sağlamak için izlenebilecek birkaç yöntem şöyle sıralanabilir:\r\n\r\nGüçlü Parolalar Kullanın: Parolalar, çevrimiçi güvenliğiniz için ilk savunma hattıdır. Güçlü bir parola, saldırganların şifrenizi tahmin etmelerini önleyerek hesabınızın güvenliğini artırır. Şifrenizde büyük/küçük harfler, sayılar ve semboller kullanarak daha güçlü bir parola oluşturabilirsiniz.\r\n\r\nGüncel Yazılımlar Kullanın: Yazılım güncellemeleri, bilgisayarınızdaki güvenlik açıklarını kapatır. Bu nedenle, bilgisayarınızda kullandığınız yazılımları ve işletim sistemini güncel tutmanız, siber güvenliğiniz için çok önemlidir.\r\n\r\nKimlik Doğrulama Yöntemleri Kullanın: İki faktörlü kimlik doğrulama, parola ile birlikte bir başka doğrulama yöntemi daha kullanarak hesabınızın güvenliğini artırır. Bu sayede, saldırganların hesabınıza erişmelerini zorlaştırarak güvenliğinizi artırabilirsiniz.\r\n\r\nVeri Yedeklemesi Yapın: Yedekleme, olası bir saldırı veya sistem hatası durumunda verilerinizin korunmasını sağlar. Verilerinizi yedekleyerek, saldırı sonrası verilerinizi geri yükleyerek kayıplarınızı en aza indirebilirsiniz.",
                    ImageUrl = "postImages/defaultImage.png",
                    ViewCount = 243,
                    CommentCount = 0,
                    LikeCount = 123,
                    UserId = 1,
                    CreatedDate = DateTime.Now,
                    UpdatedDate = DateTime.Now,
                    CreatedByUsername = "ozkky",
                    UpdatedByUsername = "ozkky",
                    IsActive = true,
                    IsDeleted = false
                },
                new()
                {
                    Id = 3 ,
                    Title = "Yapay Zeka ve Otomasyon",
                    Content = "Günümüzde teknolojinin hızlı gelişimiyle birlikte siber güvenlik tehditleri de artış göstermiştir. Siber suçlular, bireysel ve kurumsal kullanıcıların bilgi güvenliğini tehdit ederek, zararlı yazılımlar, kimlik avı saldırıları, DDoS saldırıları ve daha birçok yöntemle hedeflerine ulaşmaya çalışmaktadırlar. Bu nedenle siber güvenlik, günümüzde hayati bir öneme sahip bir konudur.\r\n\r\nSiber güvenlik tehditleri, bireysel kullanıcıların yanı sıra, işletmeler ve kurumlar için de büyük bir risk oluşturur. Siber saldırılara maruz kalan şirketler, müşteri güvenini kaybedebilir, maddi zararlarla karşılaşabilir ve hatta itibar kaybına uğrayabilirler. Bu nedenle, kurumlar siber güvenliği ciddiye almalı ve koruma önlemleri almalıdırlar.\r\n\r\nSiber güvenliği sağlamak için izlenebilecek birkaç yöntem şöyle sıralanabilir:\r\n\r\nGüçlü Parolalar Kullanın: Parolalar, çevrimiçi güvenliğiniz için ilk savunma hattıdır. Güçlü bir parola, saldırganların şifrenizi tahmin etmelerini önleyerek hesabınızın güvenliğini artırır. Şifrenizde büyük/küçük harfler, sayılar ve semboller kullanarak daha güçlü bir parola oluşturabilirsiniz.\r\n\r\nGüncel Yazılımlar Kullanın: Yazılım güncellemeleri, bilgisayarınızdaki güvenlik açıklarını kapatır. Bu nedenle, bilgisayarınızda kullandığınız yazılımları ve işletim sistemini güncel tutmanız, siber güvenliğiniz için çok önemlidir.\r\n\r\nKimlik Doğrulama Yöntemleri Kullanın: İki faktörlü kimlik doğrulama, parola ile birlikte bir başka doğrulama yöntemi daha kullanarak hesabınızın güvenliğini artırır. Bu sayede, saldırganların hesabınıza erişmelerini zorlaştırarak güvenliğinizi artırabilirsiniz.\r\n\r\nVeri Yedeklemesi Yapın: Yedekleme, olası bir saldırı veya sistem hatası durumunda verilerinizin korunmasını sağlar. Verilerinizi yedekleyerek, saldırı sonrası verilerinizi geri yükleyerek kayıplarınızı en aza indirebilirsiniz.",
                    ImageUrl = "postImages/defaultImage.png",
                    ViewCount = 222,
                    CommentCount = 0,
                    LikeCount = 134,
                    UserId = 2,
                    CreatedDate = DateTime.Now,
                    UpdatedDate = DateTime.Now,
                    CreatedByUsername = "adminuser",
                    UpdatedByUsername = "adminuser",
                    IsActive = true,
                    IsDeleted = false
                },
                new()
                {
                    Id = 4 ,
                    Title = "İnovasyon ve Yenilik",
                    Content = "Günümüzde teknolojinin hızlı gelişimiyle birlikte siber güvenlik tehditleri de artış göstermiştir. Siber suçlular, bireysel ve kurumsal kullanıcıların bilgi güvenliğini tehdit ederek, zararlı yazılımlar, kimlik avı saldırıları, DDoS saldırıları ve daha birçok yöntemle hedeflerine ulaşmaya çalışmaktadırlar. Bu nedenle siber güvenlik, günümüzde hayati bir öneme sahip bir konudur.\r\n\r\nSiber güvenlik tehditleri, bireysel kullanıcıların yanı sıra, işletmeler ve kurumlar için de büyük bir risk oluşturur. Siber saldırılara maruz kalan şirketler, müşteri güvenini kaybedebilir, maddi zararlarla karşılaşabilir ve hatta itibar kaybına uğrayabilirler. Bu nedenle, kurumlar siber güvenliği ciddiye almalı ve koruma önlemleri almalıdırlar.\r\n\r\nSiber güvenliği sağlamak için izlenebilecek birkaç yöntem şöyle sıralanabilir:\r\n\r\nGüçlü Parolalar Kullanın: Parolalar, çevrimiçi güvenliğiniz için ilk savunma hattıdır. Güçlü bir parola, saldırganların şifrenizi tahmin etmelerini önleyerek hesabınızın güvenliğini artırır. Şifrenizde büyük/küçük harfler, sayılar ve semboller kullanarak daha güçlü bir parola oluşturabilirsiniz.\r\n\r\nGüncel Yazılımlar Kullanın: Yazılım güncellemeleri, bilgisayarınızdaki güvenlik açıklarını kapatır. Bu nedenle, bilgisayarınızda kullandığınız yazılımları ve işletim sistemini güncel tutmanız, siber güvenliğiniz için çok önemlidir.\r\n\r\nKimlik Doğrulama Yöntemleri Kullanın: İki faktörlü kimlik doğrulama, parola ile birlikte bir başka doğrulama yöntemi daha kullanarak hesabınızın güvenliğini artırır. Bu sayede, saldırganların hesabınıza erişmelerini zorlaştırarak güvenliğinizi artırabilirsiniz.\r\n\r\nVeri Yedeklemesi Yapın: Yedekleme, olası bir saldırı veya sistem hatası durumunda verilerinizin korunmasını sağlar. Verilerinizi yedekleyerek, saldırı sonrası verilerinizi geri yükleyerek kayıplarınızı en aza indirebilirsiniz.",
                    ImageUrl = "postImages/defaultImage.png",
                    ViewCount = 2323,
                    CommentCount = 0,
                    LikeCount = 233,
                    UserId = 4,
                    CreatedDate = DateTime.Now,
                    UpdatedDate = DateTime.Now,
                    CreatedByUsername = "editoruser",
                    UpdatedByUsername = "editoruser",
                    IsActive = true,
                    IsDeleted = false
                },
                new()
                {
                    Id = 5 ,
                    Title = "Evde Kalmanın Keyfini Çıkarma: İç Mekan Dekorasyon Fikirleri",
                    Content = "Günümüzde evde geçirdiğimiz zamanın artması, iç mekan dekorasyonuna olan ilgiyi arttırdı. İç mekan dekorasyonu sadece evin güzel görünmesini sağlamakla kalmaz, aynı zamanda ruh halimizi ve genel sağlığımızı da etkileyebilir. Doğru renkler, aydınlatma, mobilyalar ve aksesuarlar, evinizi sıcak, rahat ve davetkar bir yer haline getirebilir.\r\n\r\nEvde kalmanın keyfini çıkarmak için, iç mekan dekorasyonunda kişisel tarzınıza uygun seçimler yapabilirsiniz. Minimalist bir tarz mı tercih ediyorsunuz yoksa renkli ve canlı bir dekor mu istiyorsunuz? Evinizdeki doğal ışığı artırmak için hangi renkler ve perdeleri tercih etmelisiniz? Tüm bu soruların yanıtlarını, evde kalmak süresince kendinize daha uygun bir yaşam alanı yaratmak için kullanabilirsiniz.\r\n\r\nUnutmayın, eviniz sizin kişisel alanınızdır ve onun size iyi hissettirmesi önemlidir. Bu nedenle, iç mekan dekorasyonunda kendi stilinizi ve kişiliğinizi yansıtan seçimler yaparak evde kalmanın keyfini çıkarabilirsiniz.",
                    ImageUrl = "postImages/defaultImage.png",
                    ViewCount = 2333,
                    CommentCount = 0,
                    LikeCount = 2113,
                    UserId = 4,
                    CreatedDate = DateTime.Now,
                    UpdatedDate = DateTime.Now,
                    CreatedByUsername = "editoruser",
                    UpdatedByUsername = "editoruser",
                    IsActive = true,
                    IsDeleted = false
                },
                new()
                {
                    Id = 6 ,
                    Title = "Sağlıklı Beslenmenin Önemi: Vitamin ve Minerallerin Günlük Hayatta Rolü",
                    Content = "Sağlıklı beslenmek, vücudumuzun ihtiyacı olan besinleri alarak sağlıklı bir yaşam sürdürmek için önemlidir. Günümüzde fast food ve işlenmiş gıdaların tüketimi arttıkça, vitamin ve mineral eksikliği gibi sağlık sorunları da artmaktadır. Vitaminler ve mineraller, vücudumuzun doğru bir şekilde çalışması için gerekli olan besin öğeleridir. Bu besinler, kemiklerin güçlendirilmesi, bağışıklık sisteminin korunması ve enerji üretimi gibi birçok vücut fonksiyonu için gereklidir.\r\n\r\nSağlıklı bir beslenme planı, yeterli miktarda vitamin ve mineral alımını içerir. C vitamini, demir, kalsiyum, magnezyum ve potasyum gibi vitamin ve mineraller, günlük diyetimizde yer alması gereken önemli besin öğeleridir. Meyve, sebze, tam tahıllı gıdalar, kuruyemişler ve tohumlar, bu besinlerin en iyi kaynaklarıdır.\r\n\r\nSağlıklı bir beslenme tarzını benimsemek, sadece fiziksel sağlığımız için değil, aynı zamanda mental sağlığımız ve yaşam kalitemiz için de önemlidir. Düzenli egzersizle birlikte sağlıklı beslenme, stres azaltmaya, zihinsel netliği artırmaya ve genel olarak daha mutlu bir yaşam sürdürmeye yardımcı olabilir.\r\n\r\nSonuç olarak, sağlıklı beslenme alışkanlıkları benimsemek, vücudumuzun ihtiyacı olan vitamin ve mineralleri alarak sağlıklı bir yaşam sürdürmek için kritik bir adımdır. Sağlıklı bir diyet, düzenli egzersiz ve yaşam tarzı değişiklikleri, sağlıklı yaşamın anahtarıdır.",
                    ImageUrl = "postImages/defaultImage.png",
                    ViewCount = 3323,
                    CommentCount = 0,
                    LikeCount = 1223,
                    UserId = 4,
                    CreatedDate = DateTime.Now,
                    UpdatedDate = DateTime.Now,
                    CreatedByUsername = "editoruser",
                    UpdatedByUsername = "editoruser",
                    IsActive = true,
                    IsDeleted = false
                },
                new()
                {
                    Id = 7 ,
                    Title = "Dijital Dünya ve Sosyal Medyanın Hayatımıza Etkisi",
                    Content = "Teknolojinin gelişmesiyle birlikte, dijital dünya ve sosyal medya hayatımızda önemli bir rol oynamaya başladı. İnternet ve sosyal medya platformları, insanlar arasındaki iletişim ve etkileşimi kolaylaştırırken, aynı zamanda yeni sorunlar ve endişeler de yaratıyor.\r\n\r\nSosyal medya, insanların arkadaşlarıyla ve aileleriyle bağlantıda kalmalarına ve dünyadaki olaylar hakkında bilgi edinmelerine yardımcı olurken, zaman zaman da zararlı etkileri olabilir. Sosyal medya kullanımı, özellikle gençler arasında, düşük özgüven, yalnızlık, kaygı ve depresyon gibi ruh sağlığı sorunlarına neden olabilir.\r\n\r\nAyrıca, sosyal medya ve dijital dünya, özellikle çocuklar ve gençler arasında, aşırı tüketim, sosyal medya bağımlılığı, internet güvenliği konularında ciddi endişelere neden oluyor. Dolayısıyla, sosyal medya ve dijital dünya ile ilgili farkındalık yaratmak, bilinçli bir şekilde kullanımını sağlamak için önemlidir.\r\n\r\nSonuç olarak, dijital dünya ve sosyal medya, hayatımızda önemli bir rol oynamaya devam edecek. Bu nedenle, insanlar olarak bu platformları nasıl kullanacağımızı öğrenmeliyiz ve özellikle gençler arasında, sorumlu ve bilinçli bir şekilde kullanımını sağlamalıyız. Sosyal medya ve dijital dünya, hayatımızın bir parçası olsa da, sağlıklı bir denge oluşturmak önemlidir.",
                    ImageUrl = "postImages/defaultImage.png",
                    ViewCount = 1223,
                    CommentCount = 0,
                    LikeCount = 123,
                    UserId = 4,
                    CreatedDate = DateTime.Now,
                    UpdatedDate = DateTime.Now,
                    CreatedByUsername = "editoruser",
                    UpdatedByUsername = "editoruser",
                    IsActive = true,
                    IsDeleted = false
                },
                new()
                {
                    Id = 8 ,
                    Title = "Düzenli Egzersizin Önemi",
                    Content = "Düzenli egzersiz yapmak, sağlıklı bir yaşam tarzının temelidir. Egzersiz yapmak, fiziksel ve zihinsel sağlığı geliştirir, kilo kontrolüne yardımcı olur, stresi azaltır, enerji seviyelerini artırır ve kalp hastalığı, diyabet ve kanser gibi birçok kronik hastalığı önlemeye yardımcı olur.\r\n\r\nAyrıca, düzenli egzersiz yapmak, kas ve kemik sağlığını korumaya yardımcı olur, vücutta yağ oranını azaltır, kan dolaşımını artırır ve bağışıklık sistemini güçlendirir. Düzenli egzersiz yapmak aynı zamanda zihinsel sağlığı da etkiler ve stres ve kaygı gibi duygusal durumları kontrol altına almaya yardımcı olur.\r\n\r\nAncak, düzenli egzersiz yapmak için zaman ve motivasyon bulmak her zaman kolay değildir. Başlangıçta, yavaş ve istikrarlı bir programla başlamak ve egzersiz hedeflerini yavaş yavaş artırmak önemlidir. Ayrıca, egzersiz programını eğlenceli hale getirmek, bir egzersiz ortağı bulmak ve farklı egzersiz türlerini denemek, motivasyonu artırabilir.\r\n\r\nSonuç olarak, düzenli egzersiz yapmak, sağlıklı bir yaşam tarzının vazgeçilmez bir parçasıdır. Egzersiz programını kişiselleştirerek, hedeflerimize uygun bir şekilde tasarlamalıyız. Egzersiz yapmak zaman ve motivasyon gerektirse de, sağlığımız için yaptığımız en iyi yatırım olduğunu unutmamalıyız.",
                    ImageUrl = "postImages/defaultImage.png",
                    ViewCount = 2324,
                    CommentCount = 0,
                    LikeCount = 222,
                    UserId = 4,
                    CreatedDate = DateTime.Now,
                    UpdatedDate = DateTime.Now,
                    CreatedByUsername = "editoruser",
                    UpdatedByUsername = "editoruser",
                    IsActive = true,
                    IsDeleted = false
                },
                new()
                {
                    Id = 9 ,
                    Title = "Çevre Sorunları ve Sürdürülebilirlik",
                    Content = "Dünya üzerindeki çevre sorunları giderek artıyor ve bu sorunların çözümü için sürdürülebilirlik kavramı hayati önem taşıyor. Sürdürülebilirlik, insanların ihtiyaçlarını karşılamak için doğal kaynakları kullanırken, gelecek nesillerin ihtiyaçlarını da göz önünde bulunduran bir yaklaşımı ifade eder.\r\n\r\nGünümüzde, iklim değişikliği, su kaynaklarının azalması, deniz kirliliği, ormansızlaşma, doğal habitatların yok olması ve atık yönetimi gibi birçok çevre sorunu var. Bu sorunların büyük bir kısmı, insan faaliyetlerinden kaynaklanmaktadır.\r\n\r\nSürdürülebilirlik, bu sorunları önlemek için birçok çözüm sunar. Örneğin, yenilenebilir enerji kaynaklarına yatırım yaparak, fosil yakıt tüketimini azaltabiliriz. Atık yönetimi programları oluşturarak, atık miktarını azaltabilir ve geri dönüşüm yaparak kaynakları yeniden kullanabiliriz. Sürdürülebilirlik aynı zamanda, tarım yöntemlerini değiştirerek, toprak erozyonunu önleyebilir ve su kaynaklarını daha etkin bir şekilde kullanabiliriz.\r\n\r\nSürdürülebilirlik, gelecek nesillerin ihtiyaçlarını da göz önünde bulundurduğu için, uzun vadeli bir yaklaşımı ifade eder. Bu nedenle, sürdürülebilirliğin önemi giderek artıyor. Herkesin bu konuda sorumluluk alması ve sürdürülebilir bir gelecek için çalışması gerekiyor.\r\n\r\nSonuç olarak, çevre sorunları ve sürdürülebilirlik, hayatımızın önemli bir parçasıdır. Sürdürülebilir bir gelecek için, herkesin sorumluluk alması ve doğal kaynakları verimli bir şekilde kullanması gerekiyor. Gelecek nesillerin de ihtiyaçlarını göz önünde bulundurarak, sürdürülebilir bir dünya için çalışmalıyız.",
                    ImageUrl = "postImages/defaultImage.png",
                    ViewCount = 223,
                    CommentCount = 0,
                    LikeCount = 23,
                    UserId = 4,
                    CreatedDate = DateTime.Now,
                    UpdatedDate = DateTime.Now,
                    CreatedByUsername = "editoruser",
                    UpdatedByUsername = "editoruser",
                    IsActive = true,
                    IsDeleted = false
                },
                new()
                {
                    Id = 10 ,
                    Title = "Dijital Dönüşüm ve İşletmeler",
                    Content = "Dijital dönüşüm, işletmelerin dijital teknolojileri kullanarak iş süreçlerini yenilemesi ve iyileştirmesi anlamına gelir. Günümüzde işletmelerin büyük bir kısmı, dijital dönüşümü benimseyerek rekabet avantajı elde etmeye çalışıyor.\r\n\r\nDijital dönüşüm, işletmelerin müşteri deneyimini geliştirmesi, verimliliği artırması ve maliyetleri düşürmesi için birçok fırsat sunar. Örneğin, bulut tabanlı yazılımlar sayesinde işletmeler, verilerini güvenli bir şekilde depolayabilir ve erişebilir hale getirebilir. Bu da işletmelerin daha hızlı ve etkili kararlar almasına yardımcı olur.\r\n\r\nDijital dönüşüm aynı zamanda, işletmelerin müşterileriyle daha iyi etkileşim kurmasını sağlar. Örneğin, sosyal medya platformları sayesinde işletmeler, müşterileriyle daha kolay ve etkili bir şekilde iletişim kurabilir ve geri bildirimleri hızlı bir şekilde alabilir. Bu da müşteri deneyimini geliştirmeye yardımcı olur ve müşteri sadakatini artırır.\r\n\r\nDijital dönüşümün bir diğer avantajı da, işletmelerin daha verimli çalışmasını sağlamasıdır. Otomasyon teknolojileri sayesinde, işletmeler manuel işlemleri otomatikleştirebilir ve çalışanların zamanını daha verimli kullanabilir. Bu da işletmelerin maliyetleri düşürmesine ve üretkenliği artırmasına yardımcı olur.\r\n\r\nSonuç olarak, dijital dönüşüm işletmeler için önemli bir fırsat sunar. İşletmeler, dijital teknolojileri benimseyerek müşteri deneyimini geliştirebilir, verimliliği artırabilir ve maliyetleri düşürebilir. Dijital dönüşümü benimseyen işletmeler, rekabet avantajı elde edebilir ve başarıya daha kolay ulaşabilir.",
                    ImageUrl = "postImages/defaultImage.png",
                    ViewCount = 2223,
                    CommentCount = 0,
                    LikeCount = 1233,
                    UserId = 2,
                    CreatedDate = DateTime.Now,
                    UpdatedDate = DateTime.Now,
                    CreatedByUsername = "adminuser",
                    UpdatedByUsername = "adminuser",
                    IsActive = true,
                    IsDeleted = false
                },
                new()
                {
                    Id = 11 ,
                    Title = "Yoga ve Zihinsel Sağlık",
                    Content = "Yoga, bedenin fiziksel ve zihinsel sağlığı için birçok faydası olan bir uygulamadır. Yoga yapmak, insanların stresi azaltmasına, zihinsel sağlığı iyileştirmesine ve genel refahı arttırmasına yardımcı olur.\r\n\r\nYoga, vücuttaki kasları ve eklemleri güçlendirir, esnekliği arttırır ve duruşu düzeltir. Bu, aynı zamanda yaralanma riskini de azaltır. Ancak yoga sadece fiziksel sağlığa fayda sağlamaz, aynı zamanda zihinsel sağlığı da destekler.\r\n\r\nYoga yapmak, kişilerin zihinsel durumunu düzeltir ve stresi azaltır. Yoga uygulayıcıları, derin nefes alarak stresi azaltır ve zihinsel berraklığı arttırır. Bu da, zihinsel sağlığı iyileştirir ve depresyon, anksiyete ve diğer zihinsel sağlık sorunlarının semptomlarını hafifletir.\r\n\r\nYoga aynı zamanda, kişilerin farkındalığını arttırır ve meditatif bir etki yaratır. Bu, düşünceleri sakinleştirir ve kişilerin daha yüksek bir bilince ulaşmasına yardımcı olur. Yoga ayrıca, uyku kalitesini de arttırır ve yorgunluğu azaltır.\r\n\r\nSonuç olarak, yoga insanların hem fiziksel hem de zihinsel sağlıklarını iyileştirmelerine yardımcı olan bir uygulamadır. Yoga yapmak, stresi azaltır, zihinsel sağlığı iyileştirir, farkındalığı arttırır ve genel refahı arttırır. Yoga, sağlıklı bir yaşam tarzı için harika bir ek olarak da kullanılabilir.",
                    ImageUrl = "postImages/defaultImage.png",
                    ViewCount = 2123,
                    CommentCount = 0,
                    LikeCount = 211,
                    UserId = 2,
                    CreatedDate = DateTime.Now,
                    UpdatedDate = DateTime.Now,
                    CreatedByUsername = "adminuser",
                    UpdatedByUsername = "adminuser",
                    IsActive = true,
                    IsDeleted = false
                },
                new()
                {
                    Id = 12 ,
                    Title = "Dijital Yorgunluk: Belirtileri ve Nasıl Önlenir?",
                    Content = "Dijital yorgunluk, sürekli olarak dijital teknolojilerle etkileşim halinde olan insanların maruz kaldığı bir durumdur. Bu, sürekli olarak telefonlara, bilgisayarlara, tablet'lere ve diğer dijital cihazlara bakarak uzun saatler geçirerek gerçekleşir.\r\n\r\nDijital yorgunluk, yorgunluk, halsizlik, baş ağrısı, gözlerde yanma ve kuru his, boyun ağrısı, uykusuzluk, konsantrasyon güçlüğü ve hatta depresyon gibi çeşitli semptomlara neden olabilir. Bu semptomlar, dijital teknolojilere aşırı maruz kalmanın bir sonucudur.\r\n\r\nDijital yorgunluğu önlemek için, bazı öneriler şunlar olabilir:\r\n\r\nDijital cihaz kullanımını azaltın. Çok fazla zaman harcamaktan kaçının.\r\nTelefonunuza veya diğer cihazlarınıza bakmak yerine, gerçek hayattaki etkinliklere katılın.\r\nİnternet kullanımını sınırlandırın. Sosyal medya ve diğer dijital platformlarda harcadığınız süreyi azaltın.\r\nKendinize ara verin. Dijital cihazlarla aranızda zaman zaman uzaklaşın ve kendi kendinizle baş başa kalın.\r\nGözlerinizi dinlendirmek için her 20-30 dakikada bir göz egzersizleri yapın.\r\nSonuç olarak, dijital yorgunluk birçok kişi için yaygın bir sorundur. Bu semptomları önlemek için, dijital cihazların kullanımını sınırlandırmak ve kendi kendinizle zaman geçirmek önemlidir. Dijital yorgunluğun önlenmesi, daha sağlıklı bir yaşam tarzını sürdürmek için önemlidir.",
                    ImageUrl = "postImages/defaultImage.png",
                    ViewCount = 2323,
                    CommentCount = 0,
                    LikeCount = 233,
                    UserId = 1,
                    CreatedDate = DateTime.Now,
                    UpdatedDate = DateTime.Now,
                    CreatedByUsername = "ozkky",
                    UpdatedByUsername = "ozkky",
                    IsActive = true,
                    IsDeleted = false
                },
            });

            builder.Entity<Category>().HasData(new Category[]
            {
                new() {
                Id = 1,
                Name = "Teknoloji",
                Description = "Dünyadan teknoloji haberleri",
                CreatedDate = DateTime.Now,
                UpdatedDate = DateTime.Now,
                CreatedByUsername = "adminuser",
                UpdatedByUsername = "adminuser",
                IsActive = true,
                IsDeleted = false
                },
                new() {
                Id = 2,
                Name = "Yazılım",
                Description = "Güncel yazılım gelişmeleri",
                CreatedDate = DateTime.Now,
                UpdatedDate = DateTime.Now,
                CreatedByUsername = "adminuser",
                UpdatedByUsername = "adminuser",
                IsActive = true,
                IsDeleted = false
                },
                new() {
                Id = 3,
                Name = "Bilim",
                Description = "Dünyadan bilim haberleri",
                CreatedDate = DateTime.Now,
                UpdatedDate = DateTime.Now,
                CreatedByUsername = "adminuser",
                UpdatedByUsername = "adminuser",
                IsActive = true,
                IsDeleted = false
                },
                new() {
                Id = 4,
                Name = "Eğitim",
                Description = "Eğitim hakkında en iyi bloglar",
                CreatedDate = DateTime.Now,
                UpdatedDate = DateTime.Now,
                CreatedByUsername = "adminuser",
                UpdatedByUsername = "adminuser",
                IsActive = true,
                IsDeleted = false
                },
                new() {
                Id = 5,
                Name = "Otomobil",
                Description = "Yerli ve yabancı otomobil haberleri",
                CreatedDate = DateTime.Now,
                UpdatedDate = DateTime.Now,
                CreatedByUsername = "adminuser",
                UpdatedByUsername = "adminuser",
                IsActive = true,
                IsDeleted = false
                },
                new() {
                Id = 6,
                Name = "Oyun",
                Description = "Yerli ve yabancı oyun sektörü haberleri",
                CreatedDate = DateTime.Now,
                UpdatedDate = DateTime.Now,
                CreatedByUsername = "adminuser",
                UpdatedByUsername = "adminuser",
                IsActive = true,
                IsDeleted = false
                },
                new() {
                Id = 7,
                Name = "Mühendislik",
                Description = "Mühendislik alanlarındaki gelişmeler",
                CreatedDate = DateTime.Now,
                UpdatedDate = DateTime.Now,
                CreatedByUsername = "adminuser",
                UpdatedByUsername = "adminuser",
                IsActive = true,
                IsDeleted = false
                },
                new() {
                Id = 8,
                Name = "Spor",
                Description = "Spor dünyasından haberler",
                CreatedDate = DateTime.Now,
                UpdatedDate = DateTime.Now,
                CreatedByUsername = "adminuser",
                UpdatedByUsername = "adminuser",
                IsActive = true,
                IsDeleted = false
                },
                new() {
                Id = 9,
                Name = "Kişisel Gelişim",
                Description = "Kişisel gelişim hakkında en iyi bloglar",
                CreatedDate = DateTime.Now,
                UpdatedDate = DateTime.Now,
                CreatedByUsername = "adminuser",
                UpdatedByUsername = "adminuser",
                IsActive = true,
                IsDeleted = false
                },
                new() {
                Id = 10,
                Name = "Sürdürülebilir Yaşam",
                Description = "Sürdürülebilir yaşam",
                CreatedDate = DateTime.Now,
                UpdatedDate = DateTime.Now,
                CreatedByUsername="adminuser",
                UpdatedByUsername = "adminuser",
                IsActive = true,
                IsDeleted = false
                },
                new() {
                Id = 11,
                Name = "Sağlıklı Yaşam",
                Description = "Dünyadan sağlıklı yaşam haberleri",
                CreatedDate = DateTime.Now,
                UpdatedDate = DateTime.Now,
                CreatedByUsername="adminuser",
                UpdatedByUsername = "adminuser",
                IsActive = true,
                IsDeleted = false
                },
                new() {
                Id = 12,
                Name = "Seyahat",
                Description = "Seyahat hakkında en iyi bloglar",
                CreatedDate = DateTime.Now,
                UpdatedDate = DateTime.Now,
                CreatedByUsername="adminuser",
                UpdatedByUsername = "adminuser",
                IsActive = true,
                IsDeleted = false},
                new() {
                Id = 13,
                Name = "Kariyer",
                Description = "Kariyerinize yön verecek içerikler",
                CreatedDate = DateTime.Now,
                UpdatedDate = DateTime.Now,
                CreatedByUsername="adminuser",
                UpdatedByUsername = "adminuser",
                IsActive = true,
                IsDeleted = false
                },
            });

            builder.Entity<BlogCategory>().HasData(new BlogCategory[]
            {
                new() { Id = 1, BlogId = 1, CategoryId = 1 },
                new() { Id = 2, BlogId = 1, CategoryId = 5 },
                new() { Id = 3, BlogId = 2, CategoryId = 1 },
                new() { Id = 4, BlogId = 3, CategoryId = 4 },
                new() { Id = 5, BlogId = 3, CategoryId = 6 },
                new() { Id = 6, BlogId = 3, CategoryId = 9 },
                new() { Id = 7, BlogId = 4, CategoryId = 8 },
                new() { Id = 8, BlogId = 5, CategoryId = 4 },
                new() { Id = 9, BlogId = 6, CategoryId = 7 },
                new() { Id = 10, BlogId = 6, CategoryId = 10 },
                new() { Id = 11, BlogId = 7, CategoryId = 12 },
                new() { Id = 12, BlogId = 8, CategoryId = 13 },
                new() { Id = 13, BlogId = 9, CategoryId = 2 },
                new() { Id = 14, BlogId = 9, CategoryId = 3 },
                new() { Id = 15, BlogId = 10, CategoryId = 1 },
                new() { Id = 16, BlogId = 10, CategoryId = 9 },
                new() { Id = 17, BlogId = 11, CategoryId = 4 },
                new() { Id = 18, BlogId = 11, CategoryId = 5 },
                new() { Id = 19, BlogId = 12, CategoryId = 11 },

            });

            builder.Entity<Comment>().HasData(new Comment[]
            {
                new()
                {
                    Id = 1,
                    Content = "Teknoloji hayatımızın ayrılmaz bir parçası haline geldi. Gelişen teknoloji ile birlikte insan hayatı daha da kolaylaşıyor ve dijital dünya daha da genişliyor.",
                    Email = "memberuser@gmail.com",
                    UserId=3,
                    Firstname="Member",
                    Lastname="Member",
                    ImageUrl="userImages/defaultUser.png",
                    BlogId = 1,
                    CreatedDate = DateTime.Now,
                    UpdatedDate = DateTime.Now,
                    CreatedByUsername="memberuser",
                    UpdatedByUsername = "memberuser",
                    IsActive = true,
                    IsDeleted = false
                },
                new()
                {
                    Id = 2,
                    Content = "Bilim, insanlık için son derece önemlidir. Bilim sayesinde dünya hakkında daha fazla şey öğreniyor ve hayatımızı daha iyi hale getiriyoruz. Bilimin gelişmesiyle birlikte, tıp, çevre, enerji ve diğer alanlarda önemli ilerlemeler kaydedildi.",
                    Email = "memberuser@gmail.com",
                    UserId=3,
                    Firstname="Member",
                    Lastname="Member",
                    ImageUrl="userImages/defaultUser.png",
                    BlogId = 2,
                    CreatedDate = DateTime.Now,
                    UpdatedDate = DateTime.Now,
                    CreatedByUsername="memberuser",
                    UpdatedByUsername = "memberuser",
                    IsActive = true,
                    IsDeleted = false
                },
                new()
                {
                    Id = 3,
                    Content = "Sanat, insanlığın ruhunu besleyen bir kaynaktır. Sanat, farklı kültürler ve farklı dönemlerden gelen insanların duygularını, hayallerini ve düşüncelerini yansıtır.",
                    Email = "editoruser@gmail.com",
                    UserId=4,
                    Firstname="Editor",
                    Lastname="Editor",
                    ImageUrl="userImages/defaultUser.png",
                    BlogId = 3,
                    CreatedDate = DateTime.Now,
                    UpdatedDate = DateTime.Now,
                    CreatedByUsername="editoruser",
                    UpdatedByUsername = "editoruser",
                    IsActive = true,
                    IsDeleted = false
                },
                new()
                {
                    Id = 4,
                    Content = "Otomobil, modern yaşamın önemli bir parçasıdır. Otomobiller, günlük hayatımızda işe, okula veya seyahat etmek için kullandığımız en yaygın taşıtlardan biridir. Ancak, otomobillerin çevresel etkileri de düşünülmesi gereken önemli bir konudur.",
                    Email = "editoruser@gmail.com",
                    UserId = 4,
                    Firstname = "Editor",
                    Lastname = "Editor",
                    ImageUrl = "userImages/defaultUser.png",
                    BlogId = 4,
                    CreatedDate = DateTime.Now,
                    UpdatedDate = DateTime.Now,
                    CreatedByUsername="editoruser",
                    UpdatedByUsername = "editoruser",
                    IsActive = true,
                    IsDeleted = false
                },
                new()
                {
                    Id = 5,
                    Content = "Sağlıklı yaşam, insan hayatı için son derece önemlidir. Sağlıklı beslenme, düzenli egzersiz ve uyku, stres yönetimi gibi faktörler, sağlıklı yaşam için önemlidir. Sağlıklı yaşam, uzun ve mutlu bir hayat için temel gereksinimdir.",
                    Email = "adminuser@gmail.com",
                    UserId = 2,
                    Firstname = "Admin",
                    Lastname = "Admin",
                    ImageUrl = "userImages/defaultUser.png",
                    BlogId = 5,
                    CreatedDate = DateTime.Now,
                    UpdatedDate = DateTime.Now,
                    CreatedByUsername="adminuser",
                    UpdatedByUsername = "adminuser",
                    IsActive = true,
                    IsDeleted = false
                },
                new()
                {
                    Id = 6,
                    Content = "Eğitim, toplumun gelişmesi için en önemli araçlardan biridir. Eğitim sayesinde insanlar, dünya hakkında daha fazla şey öğrenirler ve kendilerini geliştirirler. Eğitim, insan hayatında bir dönüm noktasıdır ve hayatımız boyunca öğrenmeye devam etmek önemlidir.",
                    Email = "adminuser@gmail.com",
                    UserId = 2,
                    Firstname = "Admin",
                    Lastname = "Admin",
                    ImageUrl = "userImages/defaultUser.png",
                    BlogId = 5,
                    CreatedDate = DateTime.Now,
                    UpdatedDate = DateTime.Now,
                    CreatedByUsername="adminuser",
                    UpdatedByUsername = "adminuser",
                    IsActive = true,
                    IsDeleted = false
                },
                new()
                {
                    Id = 7,
                    Content = "Kariyer, iş hayatında başarıya ulaşmak için önemlidir. İyi bir kariyer yapmak, finansal bağımsızlık ve kişisel tatmin açısından önemlidir. Ancak, kariyer hedefleri belirlerken, kişinin yeteneklerini ve değerlerini dikkate alması gereklidir.",
                    Email = "editoruser@gmail.com",
                    UserId=4,
                    Firstname="Editor",
                    Lastname="Editor",
                    ImageUrl="userImages/defaultUser.png",
                    BlogId = 4,
                    CreatedDate = DateTime.Now,
                    UpdatedDate = DateTime.Now,
                    CreatedByUsername="editoruser",
                    UpdatedByUsername = "editoruser",
                    IsActive = true,
                    IsDeleted = false
                },
                new()
                {
                    Id = 8,
                    Content = "Seyahat, yeni yerler keşfetmek, farklı kültürleri tanımak ve yeni deneyimler edinmek için harika bir yoldur. Seyahat etmek, kişinin bakış açısını genişletir ve yaşam deneyimini zenginleştirir. Ancak, seyahat ederken, diğer kültürlerin saygı göstermek ve doğal kaynaklara zarar vermemek gibi sorumlulukları da unutmamak gereklidir.",
                    Email = "adminuser@gmail.com",
                    UserId = 2,
                    Firstname = "Admin",
                    Lastname = "Admin",
                    ImageUrl = "userImages/defaultUser.png",
                    BlogId = 5,
                    CreatedDate = DateTime.Now,
                    UpdatedDate = DateTime.Now,
                    CreatedByUsername="adminuser",
                    UpdatedByUsername = "adminuser",
                    IsActive = true,
                    IsDeleted = false
                },
                new()
                {
                    Id = 9,
                    Content = "Eğitimde yeni metotlar, öğrenme teknikleri, öğrenci motivasyonu ve sınavlara hazırlık gibi konular hakkında paylaşılan bilgiler, öğrencilerin başarılarını arttırabilir.",
                    Email = "memberuser@gmail.com",
                    UserId=3,
                    Firstname="Member",
                    Lastname="Member",
                    ImageUrl="userImages/defaultUser.png",
                    BlogId = 6,
                    CreatedDate = DateTime.Now,
                    UpdatedDate = DateTime.Now,
                    CreatedByUsername="memberuser",
                    UpdatedByUsername = "memberuser",
                    IsActive = true,
                    IsDeleted = false
                },
                new()
                {
                    Id = 10,
                    Content = "Yeni kültürler keşfetmek, farklı yemekleri tatmak, yeni insanlarla tanışmak gibi seyahat etmenin birçok faydası var. Bu başlık altında yer alan yazılar, insanların seyahatlerini planlamalarına ve daha keyifli bir deneyim yaşamalarına yardımcı olabilir.",
                    Email = "ozkky@gmail.com",
                    UserId = 1,
                    Firstname = "Özkan",
                    Lastname = "Akkaya",
                    ImageUrl = "userImages/defaultUser.png",
                    BlogId = 7,
                    CreatedDate = DateTime.Now,
                    UpdatedDate = DateTime.Now,
                    CreatedByUsername="superadminuser",
                    UpdatedByUsername = "superadminuser",
                    IsActive = true,
                    IsDeleted = false
                },
            });

            builder.Entity<Tag>().HasData(new Tag[]
            {
                new()
                {
                Id = 1,
                Name = "Teknoloji",
                Description = "Son teknoloji haberleri ve yenilikler",
                CreatedDate = DateTime.Now,
                UpdatedDate = DateTime.Now,
                CreatedByUsername="adminuser",
                UpdatedByUsername = "adminuser",
                IsActive = true,
                IsDeleted = false
                },

                new()
                {
                Id = 2,
                Name = "Bilim Keşifleri",
                Description = "En son bilim keşifleri ve araştırmaları",
                CreatedDate = DateTime.Now,
                UpdatedDate = DateTime.Now,
                CreatedByUsername="adminuser",
                UpdatedByUsername = "adminuser",
                IsActive = true,
                IsDeleted = false
                },

                new()
                {
                Id = 3,
                Name = "Sanat Dünyası",
                Description = "Sanat dünyasındaki son gelişmeler ve haberler",
                CreatedDate = DateTime.Now,
                UpdatedDate = DateTime.Now,
                CreatedByUsername="adminuser",
                UpdatedByUsername = "adminuser",
                IsActive = true,
                IsDeleted = false
                },

                new()
                {
                Id = 4,
                Name = "Otomobil",
                Description = "En son otomobil haberleri ve yenilikler",
                CreatedDate = DateTime.Now,
                UpdatedDate = DateTime.Now,
                CreatedByUsername="adminuser",
                UpdatedByUsername = "adminuser",
                IsActive = true,
                IsDeleted = false
                },

                new()
                {
                Id = 5,
                Name = "Sağlıklı Yaşam",
                Description = "Sağlıklı yaşam, beslenme ve egzersiz tavsiyeleri",
                CreatedDate = DateTime.Now,
                UpdatedDate = DateTime.Now,
                CreatedByUsername="adminuser",
                UpdatedByUsername = "adminuser",
                IsActive = true,
                IsDeleted = false
                },

                new()
                {
                Id = 6,
                Name = "Eğitim Gündemi",
                Description = "Eğitim dünyasındaki son gelişmeler ve haberler",
                CreatedDate = DateTime.Now,
                UpdatedDate = DateTime.Now,
                CreatedByUsername="adminuser",
                UpdatedByUsername = "adminuser",
                IsActive = true,
                IsDeleted = false
                },

                new()
                {
                Id = 7,
                Name = "Kariyer",
                Description = "Kariyer yolculuğunuza rehberlik edecek tavsiyeler",
                CreatedDate = DateTime.Now,
                UpdatedDate = DateTime.Now,
                CreatedByUsername="adminuser",
                UpdatedByUsername = "adminuser",
                IsActive = true,
                IsDeleted = false
                },

                new()
                {
                Id = 8,
                Name = "Seyahat",
                Description = "Keşfedilmesi gereken seyahat rotaları ve öneriler",
                CreatedDate = DateTime.Now,
                UpdatedDate = DateTime.Now,
                CreatedByUsername="adminuser",
                UpdatedByUsername = "adminuser",
                IsActive = true,
                IsDeleted = false
                },

                new()
                {
                Id = 9,
                Name = "Bilgi Dolu Eğlence",
                Description = "Eğlendirirken bilgilendiren içerikler",
                CreatedDate = DateTime.Now,
                UpdatedDate = DateTime.Now,
                CreatedByUsername="adminuser",
                UpdatedByUsername = "adminuser",
                IsActive = true,
                IsDeleted = false
                },
                new()
                {
                Id = 10,
                Name = "Yazılım",
                Description = "Yazılım hakkında içerikler",
                CreatedDate = DateTime.Now,
                UpdatedDate = DateTime.Now,
                CreatedByUsername="adminuser",
                UpdatedByUsername = "adminuser",
                IsActive = true,
                IsDeleted = false
                },
            });

            builder.Entity<BlogTag>().HasData(new BlogTag[]
            {
                new() { Id = 1, TagId = 1, BlogId = 1 },
                new() { Id = 2, TagId = 2, BlogId = 1 },
                new() { Id = 3, TagId = 4, BlogId = 2 },
                new() { Id = 4, TagId = 6, BlogId = 2 },
                new() { Id = 5, TagId = 2, BlogId = 3 },
                new() { Id = 6, TagId = 3, BlogId = 3 },
                new() { Id = 7, TagId = 1, BlogId = 4 },
                new() { Id = 8, TagId = 9, BlogId = 4 },
                new() { Id = 9, TagId = 7, BlogId = 5 },
                new() { Id = 10, TagId = 10, BlogId = 5 },
                new() { Id = 11, TagId = 2, BlogId = 6 },
                new() { Id = 12, TagId = 3, BlogId = 7 },
                new() { Id = 13, TagId = 5, BlogId = 7 },
                new() { Id = 14, TagId = 4, BlogId = 8 },
                new() { Id = 15, TagId = 6, BlogId = 8 },
                new() { Id = 16, TagId = 7, BlogId = 9 },
                new() { Id = 17, TagId = 8, BlogId = 9 },
                new() { Id = 18, TagId = 9, BlogId = 10 },
                new() { Id = 19, TagId = 10, BlogId = 10 },
                new() { Id = 20, TagId = 2, BlogId = 11 },
                new() { Id = 21, TagId = 3, BlogId = 11 },
                new() { Id = 22, TagId = 3, BlogId = 12 },
                new() { Id = 23, TagId = 4, BlogId = 12 }
            });

        }

        private static string CreatePasswordHash(User user, string password)
        {
            var passwordHasher = new PasswordHasher<User>();
            return passwordHasher.HashPassword(user, password);
        }
    }
}

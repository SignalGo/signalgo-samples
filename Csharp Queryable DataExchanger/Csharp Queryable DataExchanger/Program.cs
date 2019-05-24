using Csharp_Queryable_DataExchanger.Models;
using SignalGo.Server.ServiceManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Csharp_Queryable_DataExchanger
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            ServerProvider serverProvider = new ServerProvider();
            serverProvider.RegisterServerService<UserService>();
            serverProvider.Start("http://localhost:6525/any");
            Console.WriteLine("server started");
            Console.ReadKey();
        }

        private static Models.FileInfo CreateFile(int id, int postId, string fileName)
        {
            return new Models.FileInfo()
            {
                Id = id,
                ContentType = System.IO.Path.GetExtension(fileName),
                FileName = System.IO.Path.GetFileName(fileName),
                Length = new System.IO.FileInfo(fileName).Length,
                PostId = postId,
            };
        }

        public static List<UserInfo> InitializeUsers()
        {
            List<UserInfo> result = new List<UserInfo>();

            result.Add(new UserInfo()
            {
                Id = 1,
                Articles = InitializeArticles(),
                Family = "Yousefi",
                Name = "Ali",
                NewsInfoes = InitializeNews(),
                Password = "213456",
                PostLikes = InitializePostLikes(),
                Posts = InitializePosts(),
                UserName = "ali.visual.studio",
                CountryName = "Iran"
            });
            InitUser(result.Last());

            result.Add(new UserInfo()
            {
                Id = 2,
                Articles = InitializeArticles(),
                Family = "Jamal",
                Name = "Jak",
                NewsInfoes = InitializeNews(),
                Password = "154722@",
                PostLikes = InitializePostLikes(),
                Posts = InitializePosts(),
                UserName = "jak.visual.studio",
                CountryName = "italia"
            });
            InitUser(result.Last());

            result.Add(new UserInfo()
            {
                Id = 3,
                Articles = InitializeArticles(),
                Family = "Sista",
                Name = "Gerardo",
                NewsInfoes = InitializeNews(),
                Password = "9157225$",
                PostLikes = InitializePostLikes(),
                Posts = InitializePosts(),
                UserName = "gerardo.sista",
                CountryName = "italia"
            });
            InitUser(result.Last());

            result.Add(new UserInfo()
            {
                Id = 4,
                Articles = InitializeArticles(),
                Family = "Yousefi",
                Name = "Mohammad",
                NewsInfoes = InitializeNews(),
                Password = "9522",
                PostLikes = InitializePostLikes(),
                Posts = InitializePosts(),
                UserName = "mohammad.sista",
                CountryName = "Iran"
            });
            InitUser(result.Last());

            result.Add(new UserInfo()
            {
                Id = 5,
                Articles = InitializeArticles(),
                Family = "Jakson",
                Name = "Axel",
                NewsInfoes = InitializeNews(),
                Password = "hg15448",
                PostLikes = InitializePostLikes(),
                Posts = InitializePosts(),
                UserName = "axel123",
                CountryName = "USA"
            });
            InitUser(result.Last());
            return result;
        }

        private static void InitUser(UserInfo userInfo)
        {
            userInfo.Articles = userInfo.Articles.Where(x => x.UserId == userInfo.Id).ToList();
            foreach (ArticleInfo item in userInfo.Articles)
            {
                item.UserInfo = userInfo;
                item.UserId = userInfo.Id;
            }

            userInfo.NewsInfoes = userInfo.NewsInfoes.Where(x => x.UserId == userInfo.Id).ToList();
            foreach (NewsInfo item in userInfo.NewsInfoes)
            {
                item.UserInfo = userInfo;
                item.UserId = userInfo.Id;
            }

            userInfo.PostLikes = userInfo.PostLikes.Where(x => x.UserId == userInfo.Id).ToList();
            foreach (PostLikeInfo item in userInfo.PostLikes)
            {
                item.UserInfo = userInfo;
                item.UserId = userInfo.Id;
            }

            userInfo.Posts = userInfo.Posts.Where(x => x.UserId == userInfo.Id).ToList();
            foreach (PostInfo item in userInfo.Posts)
            {
                item.UserInfo = userInfo;
                item.UserId = userInfo.Id;
            }
        }

        private static void InitPost(PostInfo postInfo)
        {
            postInfo.Files = postInfo.Files.Where(x => x.PostId == postInfo.Id).ToList();
            foreach (var item in postInfo.Files)
            {
                item.PostId = postInfo.Id;
                item.PostInfo = postInfo;
            }
            postInfo.PostLikes = InitializePostLikes().Where(x => x.PostId == postInfo.Id).ToList();
            foreach (var item in postInfo.PostLikes)
            {
                item.PostInfo = postInfo;
            }
        }

        private static List<PostInfo> InitializePosts()
        {
            List<PostInfo> result = new List<PostInfo>();

            result.Add(new PostInfo()
            {
                Id = 1,
                PostLikes = InitializePostLikes(),
                Title = "Good days",
                Content = "good days in iran is not very hot!",
                Files = InitializeFiles(),
                UserId = 1
            });
            InitPost(result.Last());
            result.Add(new PostInfo()
            {
                Id = 2,
                PostLikes = InitializePostLikes(),
                Title = "Bad days",
                Content = "bad days in iran is not very bad!",
                Files = InitializeFiles(),
                UserId = 1
            });
            InitPost(result.Last());
            result.Add(new PostInfo()
            {
                Id = 3,
                PostLikes = InitializePostLikes(),
                Title = "Normal days",
                Content = "normal days in iran is not very normal!",
                Files = InitializeFiles(),
                UserId = 1
            });
            InitPost(result.Last());
            result.Add(new PostInfo()
            {
                Id = 4,
                PostLikes = InitializePostLikes(),
                Title = "3 days without signalgo",
                Content = "after 3 days without signalgo you could see how developers are sad!",
                Files = InitializeFiles(),
                UserId = 1
            });
            InitPost(result.Last());
            result.Add(new PostInfo()
            {
                Id = 5,
                PostLikes = InitializePostLikes(),
                Title = "how to start signalgo",
                Content = "open https://github.com/SignalGo/SignalGo-full-net and you can see documents of signalgo",
                Files = InitializeFiles()
            });
            InitPost(result.Last());
            result.Add(new PostInfo()
            {
                Id = 6,
                PostLikes = InitializePostLikes(),
                Title = "C#",
                Content = "C# is good programming language",
                Files = InitializeFiles(),
                UserId = 1
            });
            InitPost(result.Last());
            result.Add(new PostInfo()
            {
                Id = 7,
                PostLikes = InitializePostLikes(),
                Title = "Java",
                Content = "java is not support to get type of generic in generic class at runtime!",
                Files = InitializeFiles(),
                UserId = 2
            });
            InitPost(result.Last());
            result.Add(new PostInfo()
            {
                Id = 8,
                PostLikes = InitializePostLikes(),
                Title = "Life in Iran",
                Content = "Programmers in iran hard working with low salary!",
                Files = InitializeFiles(),
                UserId = 2
            });
            InitPost(result.Last());
            result.Add(new PostInfo()
            {
                Id = 9,
                PostLikes = InitializePostLikes(),
                Title = "Programmers",
                Content = "Programmers are smart peoples!",
                Files = InitializeFiles(),
                UserId = 3

            });
            InitPost(result.Last());
            result.Add(new PostInfo()
            {
                Id = 10,
                PostLikes = InitializePostLikes(),
                Title = "Full stack developers",
                Content = "full stack developers know about programming languages and they know what language make them faster!",
                Files = InitializeFiles(),
                UserId = 2
            });
            InitPost(result.Last());
            result.Add(new PostInfo()
            {
                Id = 11,
                PostLikes = InitializePostLikes(),
                Title = "Persian English",
                Content = "Persian english is new english language when some peoples like me don't know english as good, they will use persian english language!",
                Files = InitializeFiles(),
                UserId = 4
            });
            InitPost(result.Last());
            return result;
        }

        private static List<NewsInfo> InitializeNews()
        {
            List<NewsInfo> result = new List<NewsInfo>();

            result.Add(new NewsInfo()
            {
                Id = 1,
                Date = DateTime.Now.AddDays(-1),
                Title = "Good days news",
                Body = "good days in iran is not very hot! news",
                UserId = 2
            });
            result.Add(new NewsInfo()
            {
                Id = 2,
                Date = DateTime.Now.AddDays(-2),
                Title = "Bad days news",
                Body = "bad days in iran is not very bad! news",
                UserId = 2
            });
            result.Add(new NewsInfo()
            {
                Id = 3,
                Date = DateTime.Now.AddDays(-2),
                Title = "Normal days news",
                Body = "normal days in iran is not very normal! news",
                UserId = 2
            });
            result.Add(new NewsInfo()
            {
                Id = 4,
                Date = DateTime.Now.AddDays(-2),
                Title = "3 days without signalgo news",
                Body = "after 3 days without signalgo you could see how developers are sad! news",
                UserId = 2
            });
            result.Add(new NewsInfo()
            {
                Id = 5,
                Date = DateTime.Now.AddDays(-3),
                Title = "how to start signalgo news",
                Body = "open https://github.com/SignalGo/SignalGo-full-net and you can see documents of signalgo news",
                UserId = 3
            });
            result.Add(new NewsInfo()
            {
                Id = 6,
                Date = DateTime.Now.AddDays(-3),
                Title = "C# news",
                Body = "C# is good programming language news",
                UserId = 3
            });
            result.Add(new NewsInfo()
            {
                Id = 7,
                Date = DateTime.Now.AddDays(-4),
                Title = "Java news",
                Body = "java is not support to get type of generic in generic class at runtime! news",
                UserId = 3
            });
            result.Add(new NewsInfo()
            {
                Id = 8,
                Date = DateTime.Now.AddDays(-5),
                Title = "Life in Iran news",
                Body = "Programmers in iran hard working with low salary! news",
                UserId = 3
            });
            result.Add(new NewsInfo()
            {
                Id = 9,
                Date = DateTime.Now.AddDays(-6),
                Title = "Programmers news",
                Body = "Programmers are smart peoples! news",
                UserId = 3
            });
            result.Add(new NewsInfo()
            {
                Id = 10,
                Date = DateTime.Now.AddDays(-6),
                Title = "Full stack developers news",
                Body = "full stack developers know about programming languages and they know what language make them faster! news",
                UserId = 4
            });
            result.Add(new NewsInfo()
            {
                Id = 11,
                Date = DateTime.Now.AddDays(-10),
                Title = "Persian English news",
                Body = "Persian english is new english language when some peoples like me don't know english as good, they will use persian english language! news",
                UserId = 5
            });
            return result;
        }

        private static List<ArticleInfo> InitializeArticles()
        {
            List<ArticleInfo> result = new List<ArticleInfo>();

            result.Add(new ArticleInfo()
            {
                Id = 1,
                Date = DateTime.Now.AddDays(-1),
                Title = "Good days article",
                Content = "good days in iran is not very hot! article",
                UserId = 2
            });
            result.Add(new ArticleInfo()
            {
                Id = 2,
                Date = DateTime.Now.AddDays(-2),
                Title = "Bad days article",
                Content = "bad days in iran is not very bad! article",
                UserId = 2
            });
            result.Add(new ArticleInfo()
            {
                Id = 3,
                Date = DateTime.Now.AddDays(-2),
                Title = "Normal days article",
                Content = "normal days in iran is not very normal! article",
                UserId = 2
            });
            result.Add(new ArticleInfo()
            {
                Id = 4,
                Date = DateTime.Now.AddDays(-2),
                Title = "3 days without signalgo article",
                Content = "after 3 days without signalgo you could see how developers are sad! article",
                UserId = 2
            });
            result.Add(new ArticleInfo()
            {
                Id = 5,
                Date = DateTime.Now.AddDays(-3),
                Title = "how to start signalgo article",
                Content = "open https://github.com/SignalGo/SignalGo-full-net and you can see documents of signalgo article",
                UserId = 3
            });
            result.Add(new ArticleInfo()
            {
                Id = 6,
                Date = DateTime.Now.AddDays(-3),
                Title = "C# article",
                Content = "C# is good programming language article",
                UserId = 3
            });
            result.Add(new ArticleInfo()
            {
                Id = 7,
                Date = DateTime.Now.AddDays(-4),
                Title = "Java article",
                Content = "java is not support to get type of generic in generic class at runtime! article",
                UserId = 3
            });
            result.Add(new ArticleInfo()
            {
                Id = 8,
                Date = DateTime.Now.AddDays(-5),
                Title = "Life in Iran article",
                Content = "Programmers in iran hard working with low salary! article",
                UserId = 3
            });
            result.Add(new ArticleInfo()
            {
                Id = 9,
                Date = DateTime.Now.AddDays(-6),
                Title = "Programmers article",
                Content = "Programmers are smart peoples! article",
                UserId = 3
            });
            result.Add(new ArticleInfo()
            {
                Id = 10,
                Date = DateTime.Now.AddDays(-6),
                Title = "Full stack developers article",
                Content = "full stack developers know about programming languages and they know what language make them faster! article",
                UserId = 4
            });
            result.Add(new ArticleInfo()
            {
                Id = 11,
                Date = DateTime.Now.AddDays(-10),
                Title = "Persian English article",
                Content = "Persian english is new english language when some peoples like me don't know english as good, they will use persian english language! article",
                UserId = 5
            });
            return result;
        }

        private static List<PostLikeInfo> InitializePostLikes()
        {
            List<PostLikeInfo> result = new List<PostLikeInfo>();

            result.Add(new PostLikeInfo()
            {
                Id = 1,
                PostId = 5,
                UserId = 4
            });
            result.Add(new PostLikeInfo()
            {
                Id = 1,
                PostId = 6,
                UserId = 4
            });
            result.Add(new PostLikeInfo()
            {
                Id = 2,
                PostId = 11,
                UserId = 1
            });
            result.Add(new PostLikeInfo()
            {
                Id = 3,
                PostId = 11,
                UserId = 2
            });
            return result;
        }

        private static List<FileInfo> InitializeFiles()
        {
            List<FileInfo> result = new List<FileInfo>();

            result.Add(new FileInfo()
            {
                Id = 1,
                ContentType = ".img",
                FileName = "cache.img",
                Length = 69206016,
                PostId = 10
            });
            result.Add(new FileInfo()
            {
                Id = 1,
                ContentType = ".ini",
                FileName = "config.ini",
                Length = 566,
                PostId = 6
            });
            result.Add(new FileInfo()
            {
                Id = 1,
                ContentType = ".ini",
                FileName = "emulator-user.ini",
                Length = 53,
                PostId = 9
            });
            result.Add(new FileInfo()
            {
                Id = 1,
                ContentType = ".ini",
                FileName = "hardware-qemu.ini",
                Length = 1657,
                PostId = 4
            });
            result.Add(new FileInfo()
            {
                Id = 1,
                ContentType = ".img",
                FileName = "sdcard.img",
                Length = 104857600,
                PostId = 4
            });
            result.Add(new FileInfo()
            {
                Id = 1,
                ContentType = ".img",
                FileName = "userdata-qemu.img",
                Length = 2147483648,
                PostId = 11
            });
            result.Add(new FileInfo()
            {
                Id = 1,
                ContentType = ".img",
                FileName = "userdata.img",
                Length = 2147483648,
                PostId = 4
            });
            result.Add(new FileInfo()
            {
                Id = 1,
                ContentType = ".img",
                FileName = "cache.img",
                Length = 69206016,
                PostId = 1
            });
            result.Add(new FileInfo()
            {
                Id = 1,
                ContentType = ".ini",
                FileName = "config.ini",
                Length = 523,
                PostId = 11
            });
            result.Add(new FileInfo()
            {
                Id = 1,
                ContentType = ".ini",
                FileName = "emulator-user.ini",
                Length = 55,
                PostId = 8
            });
            result.Add(new FileInfo()
            {
                Id = 1,
                ContentType = ".ini",
                FileName = "hardware-qemu.ini",
                Length = 1701,
                PostId = 9
            });
            result.Add(new FileInfo()
            {
                Id = 1,
                ContentType = ".img",
                FileName = "sdcard.img",
                Length = 536870912,
                PostId = 4
            });
            result.Add(new FileInfo()
            {
                Id = 1,
                ContentType = ".img",
                FileName = "userdata-qemu.img",
                Length = 2147483648,
                PostId = 8
            });
            result.Add(new FileInfo()
            {
                Id = 1,
                ContentType = ".img",
                FileName = "userdata.img",
                Length = 2147483648,
                PostId = 5
            });
            result.Add(new FileInfo()
            {
                Id = 1,
                ContentType = ".conf",
                FileName = "AVD.conf",
                Length = 71,
                PostId = 10
            });
            result.Add(new FileInfo()
            {
                Id = 1,
                ContentType = ".img",
                FileName = "cache.img",
                Length = 69206016,
                PostId = 10
            });
            result.Add(new FileInfo()
            {
                Id = 1,
                ContentType = ".qcow2",
                FileName = "cache.img.qcow2",
                Length = 590336,
                PostId = 9
            });
            result.Add(new FileInfo()
            {
                Id = 1,
                ContentType = ".ini",
                FileName = "config.ini",
                Length = 719,
                PostId = 9
            });
            result.Add(new FileInfo()
            {
                Id = 1,
                ContentType = ".ini",
                FileName = "emulator-user.ini",
                Length = 54,
                PostId = 7
            });
            result.Add(new FileInfo()
            {
                Id = 1,
                ContentType = ".ini",
                FileName = "hardware-qemu.ini",
                Length = 1995,
                PostId = 2
            });
            result.Add(new FileInfo()
            {
                Id = 1,
                ContentType = ".img",
                FileName = "sdcard.img",
                Length = 524288000,
                PostId = 10
            });
            result.Add(new FileInfo()
            {
                Id = 1,
                ContentType = ".qcow2",
                FileName = "sdcard.img.qcow2",
                Length = 1245184,
                PostId = 10
            });
            result.Add(new FileInfo()
            {
                Id = 1,
                ContentType = ".img",
                FileName = "userdata-qemu.img",
                Length = 576716800,
                PostId = 11
            });
            result.Add(new FileInfo()
            {
                Id = 1,
                ContentType = ".qcow2",
                FileName = "userdata-qemu.img.qcow2",
                Length = 17564160,
                PostId = 8
            });
            result.Add(new FileInfo()
            {
                Id = 1,
                ContentType = ".img",
                FileName = "userdata.img",
                Length = 576716800,
                PostId = 4
            });
            result.Add(new FileInfo()
            {
                Id = 1,
                ContentType = ".cache",
                FileName = "version_num.cache",
                Length = 9,
                PostId = 1
            });
            result.Add(new FileInfo()
            {
                Id = 1,
                ContentType = ".bin",
                FileName = "pstore.bin",
                Length = 65536,
                PostId = 10
            });
            result.Add(new FileInfo()
            {
                Id = 1,
                ContentType = ".pb",
                FileName = "snapshot_deps.pb",
                Length = 0,
                PostId = 3
            });
            result.Add(new FileInfo()
            {
                Id = 1,
                ContentType = ".ini",
                FileName = "hardware.ini",
                Length = 1995,
                PostId = 3
            });
            result.Add(new FileInfo()
            {
                Id = 1,
                ContentType = ".bin",
                FileName = "ram.bin",
                Length = 286872176,
                PostId = 8
            });
            result.Add(new FileInfo()
            {
                Id = 1,
                ContentType = ".png",
                FileName = "screenshot.png",
                Length = 310218,
                PostId = 2
            });
            result.Add(new FileInfo()
            {
                Id = 1,
                ContentType = ".pb",
                FileName = "snapshot.pb",
                Length = 866,
                PostId = 9
            });
            result.Add(new FileInfo()
            {
                Id = 1,
                ContentType = ".bin",
                FileName = "textures.bin",
                Length = 5291229,
                PostId = 3
            });
            result.Add(new FileInfo()
            {
                Id = 1,
                ContentType = ".conf",
                FileName = "AVD.conf",
                Length = 102,
                PostId = 11
            });
            result.Add(new FileInfo()
            {
                Id = 1,
                ContentType = ".img",
                FileName = "cache.img",
                Length = 69206016,
                PostId = 10
            });
            result.Add(new FileInfo()
            {
                Id = 1,
                ContentType = ".qcow2",
                FileName = "cache.img.qcow2",
                Length = 9175040,
                PostId = 2
            });
            result.Add(new FileInfo()
            {
                Id = 1,
                ContentType = ".ini",
                FileName = "config.ini",
                Length = 701,
                PostId = 2
            });
            result.Add(new FileInfo()
            {
                Id = 1,
                ContentType = ".txt",
                FileName = "emu-launch-params.txt",
                Length = 139,
                PostId = 4
            });
            result.Add(new FileInfo()
            {
                Id = 1,
                ContentType = ".ini",
                FileName = "emulator-user.ini",
                Length = 53,
                PostId = 9
            });
            result.Add(new FileInfo()
            {
                Id = 1,
                ContentType = ".img",
                FileName = "encryptionkey.img",
                Length = 1048576,
                PostId = 11
            });
            result.Add(new FileInfo()
            {
                Id = 1,
                ContentType = ".qcow2",
                FileName = "encryptionkey.img.qcow2",
                Length = 459264,
                PostId = 6
            });
            result.Add(new FileInfo()
            {
                Id = 1,
                ContentType = ".ini",
                FileName = "hardware-qemu.ini",
                Length = 1959,
                PostId = 8
            });
            result.Add(new FileInfo()
            {
                Id = 1,
                ContentType = ".lock",
                FileName = "multiinstance.lock",
                Length = 0,
                PostId = 7
            });
            result.Add(new FileInfo()
            {
                Id = 1,
                ContentType = ".ini",
                FileName = "quickbootChoice.ini",
                Length = 19,
                PostId = 3
            });
            result.Add(new FileInfo()
            {
                Id = 1,
                ContentType = ".txt",
                FileName = "read-snapshot.txt",
                Length = 0,
                PostId = 8
            });
            result.Add(new FileInfo()
            {
                Id = 1,
                ContentType = ".img",
                FileName = "sdcard.img",
                Length = 524288000,
                PostId = 1
            });
            result.Add(new FileInfo()
            {
                Id = 1,
                ContentType = ".qcow2",
                FileName = "sdcard.img.qcow2",
                Length = 1048576,
                PostId = 5
            });
            result.Add(new FileInfo()
            {
                Id = 1,
                ContentType = ".img",
                FileName = "userdata-qemu.img",
                Length = 576716800,
                PostId = 9
            });
            result.Add(new FileInfo()
            {
                Id = 1,
                ContentType = ".qcow2",
                FileName = "userdata-qemu.img.qcow2",
                Length = 905576448,
                PostId = 6
            });
            result.Add(new FileInfo()
            {
                Id = 1,
                ContentType = ".img",
                FileName = "userdata.img",
                Length = 576716800,
                PostId = 3
            });
            result.Add(new FileInfo()
            {
                Id = 1,
                ContentType = ".cache",
                FileName = "version_num.cache",
                Length = 9,
                PostId = 4
            });
            result.Add(new FileInfo()
            {
                Id = 1,
                ContentType = ".bin",
                FileName = "pstore.bin",
                Length = 65536,
                PostId = 6
            });
            result.Add(new FileInfo()
            {
                Id = 1,
                ContentType = ".pb",
                FileName = "snapshot_deps.pb",
                Length = 0,
                PostId = 11
            });
            result.Add(new FileInfo()
            {
                Id = 1,
                ContentType = ".ini",
                FileName = "hardware.ini",
                Length = 1959,
                PostId = 5
            });
            result.Add(new FileInfo()
            {
                Id = 1,
                ContentType = ".bin",
                FileName = "ram.bin",
                Length = 280,
                PostId = 8
            });
            result.Add(new FileInfo()
            {
                Id = 1,
                ContentType = ".img",
                FileName = "ram.img",
                Length = 1999699968,
                PostId = 1
            });
            result.Add(new FileInfo()
            {
                Id = 1,
                ContentType = ".png",
                FileName = "screenshot.png",
                Length = 23045,
                PostId = 7
            });
            result.Add(new FileInfo()
            {
                Id = 1,
                ContentType = ".pb",
                FileName = "snapshot.pb",
                Length = 854,
                PostId = 2
            });
            result.Add(new FileInfo()
            {
                Id = 1,
                ContentType = ".bin",
                FileName = "textures.bin",
                Length = 622708,
                PostId = 11
            });
            result.Add(new FileInfo()
            {
                Id = 1,
                ContentType = ".conf",
                FileName = "AVD.conf",
                Length = 102,
                PostId = 4
            });
            result.Add(new FileInfo()
            {
                Id = 1,
                ContentType = ".img",
                FileName = "cache.img",
                Length = 69206016,
                PostId = 1
            });
            result.Add(new FileInfo()
            {
                Id = 1,
                ContentType = ".qcow2",
                FileName = "cache.img.qcow2",
                Length = 4259840,
                PostId = 2
            });
            result.Add(new FileInfo()
            {
                Id = 1,
                ContentType = ".ini",
                FileName = "config.ini",
                Length = 912,
                PostId = 10
            });
            result.Add(new FileInfo()
            {
                Id = 1,
                ContentType = ".txt",
                FileName = "emu-launch-params.txt",
                Length = 274,
                PostId = 4
            });
            result.Add(new FileInfo()
            {
                Id = 1,
                ContentType = ".ini",
                FileName = "emulator-user.ini",
                Length = 55,
                PostId = 7
            });
            result.Add(new FileInfo()
            {
                Id = 1,
                ContentType = ".img",
                FileName = "encryptionkey.img",
                Length = 1048576,
                PostId = 1
            });
            result.Add(new FileInfo()
            {
                Id = 1,
                ContentType = ".qcow2",
                FileName = "encryptionkey.img.qcow2",
                Length = 459264,
                PostId = 2
            });
            result.Add(new FileInfo()
            {
                Id = 1,
                ContentType = ".ini",
                FileName = "hardware-qemu.ini",
                Length = 2096,
                PostId = 9
            });
            result.Add(new FileInfo()
            {
                Id = 1,
                ContentType = ".lock",
                FileName = "multiinstance.lock",
                Length = 0,
                PostId = 6
            });
            result.Add(new FileInfo()
            {
                Id = 1,
                ContentType = ".ini",
                FileName = "quickbootChoice.ini",
                Length = 19,
                PostId = 8
            });
            result.Add(new FileInfo()
            {
                Id = 1,
                ContentType = ".txt",
                FileName = "read-snapshot.txt",
                Length = 0,
                PostId = 8
            });
            result.Add(new FileInfo()
            {
                Id = 1,
                ContentType = ".img",
                FileName = "userdata-qemu.img",
                Length = 6442450944,
                PostId = 6
            });
            result.Add(new FileInfo()
            {
                Id = 1,
                ContentType = ".qcow2",
                FileName = "userdata-qemu.img.qcow2",
                Length = 4586340352,
                PostId = 1
            });
            result.Add(new FileInfo()
            {
                Id = 1,
                ContentType = ".img",
                FileName = "userdata.img",
                Length = 1048576,
                PostId = 7
            });
            result.Add(new FileInfo()
            {
                Id = 1,
                ContentType = ".cache",
                FileName = "version_num.cache",
                Length = 9,
                PostId = 8
            });
            result.Add(new FileInfo()
            {
                Id = 1,
                ContentType = ".bin",
                FileName = "pstore.bin",
                Length = 65536,
                PostId = 2
            });
            result.Add(new FileInfo()
            {
                Id = 1,
                ContentType = ".pb",
                FileName = "snapshot_deps.pb",
                Length = 0,
                PostId = 3
            });
            result.Add(new FileInfo()
            {
                Id = 1,
                ContentType = ".ini",
                FileName = "hardware.ini",
                Length = 2096,
                PostId = 8
            });
            result.Add(new FileInfo()
            {
                Id = 1,
                ContentType = ".bin",
                FileName = "ram.bin",
                Length = 280,
                PostId = 11
            });
            result.Add(new FileInfo()
            {
                Id = 1,
                ContentType = ".img",
                FileName = "ram.img",
                Length = 1999699968,
                PostId = 4
            });
            result.Add(new FileInfo()
            {
                Id = 1,
                ContentType = ".png",
                FileName = "screenshot.png",
                Length = 342239,
                PostId = 7
            });
            result.Add(new FileInfo()
            {
                Id = 1,
                ContentType = ".pb",
                FileName = "snapshot.pb",
                Length = 803,
                PostId = 2
            });
            result.Add(new FileInfo()
            {
                Id = 1,
                ContentType = ".bin",
                FileName = "textures.bin",
                Length = 6047450,
                PostId = 8
            });
            result.Add(new FileInfo()
            {
                Id = 1,
                ContentType = "",
                FileName = "adbcommand0a2e2d6b-2545-4977-a36f-d926dbd68baf",
                Length = 816757,
                PostId = 11
            });
            result.Add(new FileInfo()
            {
                Id = 1,
                ContentType = "",
                FileName = "adbcommand1a1decfb-5cbe-485f-964a-4489f15903d1",
                Length = 817815,
                PostId = 4
            });
            result.Add(new FileInfo()
            {
                Id = 1,
                ContentType = "",
                FileName = "adbcommand2ce0dba2-8bfb-44b0-9608-2d12a637bf9e",
                Length = 15,
                PostId = 8
            });
            result.Add(new FileInfo()
            {
                Id = 1,
                ContentType = "",
                FileName = "adbcommand2f666710-077a-4218-a252-9d86c1b98e67",
                Length = 817335,
                PostId = 9
            });
            result.Add(new FileInfo()
            {
                Id = 1,
                ContentType = "",
                FileName = "adbcommand7d85e2c1-d109-492a-9610-0b77154b1fa9",
                Length = 816680,
                PostId = 9
            });
            result.Add(new FileInfo()
            {
                Id = 1,
                ContentType = "",
                FileName = "adbcommandd3cb90d1-5b06-44b7-bb39-169b715a2da2",
                Length = 15,
                PostId = 3
            });
            result.Add(new FileInfo()
            {
                Id = 1,
                ContentType = "",
                FileName = "adbcommandd6c7679e-0d5e-4cb6-8762-82a2720a1e19",
                Length = 15,
                PostId = 10
            });
            result.Add(new FileInfo()
            {
                Id = 1,
                ContentType = "",
                FileName = "adbcommanddb96c7bc-7567-49d9-a951-8f210a8c2ab3",
                Length = 15,
                PostId = 7
            });
            result.Add(new FileInfo()
            {
                Id = 1,
                ContentType = ".img",
                FileName = "cache.img",
                Length = 69206016,
                PostId = 1
            });
            result.Add(new FileInfo()
            {
                Id = 1,
                ContentType = ".ini",
                FileName = "config.ini",
                Length = 674,
                PostId = 4
            });
            result.Add(new FileInfo()
            {
                Id = 1,
                ContentType = ".ini",
                FileName = "emulator-user.ini",
                Length = 54,
                PostId = 5
            });
            result.Add(new FileInfo()
            {
                Id = 1,
                ContentType = ".ini",
                FileName = "hardware-qemu.ini",
                Length = 1660,
                PostId = 10
            });
            result.Add(new FileInfo()
            {
                Id = 1,
                ContentType = ".img",
                FileName = "sdcard.img",
                Length = 104857600,
                PostId = 4
            });
            result.Add(new FileInfo()
            {
                Id = 1,
                ContentType = ".img",
                FileName = "userdata-qemu.img",
                Length = 2147483648,
                PostId = 10
            });
            result.Add(new FileInfo()
            {
                Id = 1,
                ContentType = ".img",
                FileName = "userdata.img",
                Length = 2147483648,
                PostId = 4
            });
            result.Add(new FileInfo()
            {
                Id = 1,
                ContentType = ".img",
                FileName = "cache.img",
                Length = 69206016,
                PostId = 8
            });
            result.Add(new FileInfo()
            {
                Id = 1,
                ContentType = ".ini",
                FileName = "config.ini",
                Length = 566,
                PostId = 8
            });
            result.Add(new FileInfo()
            {
                Id = 1,
                ContentType = ".ini",
                FileName = "emulator-user.ini",
                Length = 54,
                PostId = 11
            });
            result.Add(new FileInfo()
            {
                Id = 1,
                ContentType = ".ini",
                FileName = "hardware-qemu.ini",
                Length = 1754,
                PostId = 7
            });
            result.Add(new FileInfo()
            {
                Id = 1,
                ContentType = ".img",
                FileName = "sdcard.img",
                Length = 536870912,
                PostId = 3
            });
            result.Add(new FileInfo()
            {
                Id = 1,
                ContentType = ".img",
                FileName = "userdata-qemu.img",
                Length = 2147483648,
                PostId = 9
            });
            result.Add(new FileInfo()
            {
                Id = 1,
                ContentType = ".img",
                FileName = "userdata.img",
                Length = 2147483648,
                PostId = 7
            });
            result.Add(new FileInfo()
            {
                Id = 1,
                ContentType = ".img",
                FileName = "cache.img",
                Length = 69206016,
                PostId = 9
            });
            result.Add(new FileInfo()
            {
                Id = 1,
                ContentType = ".ini",
                FileName = "config.ini",
                Length = 566,
                PostId = 6
            });
            result.Add(new FileInfo()
            {
                Id = 1,
                ContentType = ".ini",
                FileName = "emulator-user.ini",
                Length = 53,
                PostId = 4
            });
            result.Add(new FileInfo()
            {
                Id = 1,
                ContentType = ".ini",
                FileName = "hardware-qemu.ini",
                Length = 1758,
                PostId = 9
            });
            result.Add(new FileInfo()
            {
                Id = 1,
                ContentType = ".img",
                FileName = "sdcard.img",
                Length = 536870912,
                PostId = 7
            });
            result.Add(new FileInfo()
            {
                Id = 1,
                ContentType = ".img",
                FileName = "userdata-qemu.img",
                Length = 2147483648,
                PostId = 9
            });
            result.Add(new FileInfo()
            {
                Id = 1,
                ContentType = ".img",
                FileName = "userdata.img",
                Length = 2147483648,
                PostId = 10
            });

            return result;
        }
    }
}

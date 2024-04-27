using AmadonStandardLib.Classes;
using LibGit2Sharp;
using LibGit2Sharp.Handlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;



namespace AmadonStandardLib.Helpers
{
    public class GitHelper
    {
        private static GitHelper _githelper = new GitHelper();

        public static GitHelper Instance { get { return _githelper; } }

        private string? RepoPath = null;


        private CredentialsHandler Credentials(string username, string password)
        {
            return new CredentialsHandler(
                    (url, usernameFromUrl, types) =>
                        new UsernamePasswordCredentials()
                        {
                            Username = "username",
                            Password = "password"
                        });
        }

        private bool Fetch(Repository repo)
        {
            try
            {
                var options = new FetchOptions();
                options.Prune = true;
                options.TagFetchMode = TagFetchMode.Auto;
                //options.CredentialsProvider = Credentials(string username, string password)
                Remote remote = repo.Network.Remotes["origin"];
                string msg = "Fetching remote";
                IEnumerable<string> refSpecs = remote.FetchRefSpecs.Select(x => x.Specification);
                Commands.Fetch(repo, remote.Name, refSpecs, options, msg);
                return true;
            }
            catch (Exception ex)
            {
                LibraryEventsControl.FireSendUserAndLogMessage($"Fetch Error, repository= {repo.Head.FriendlyName}: ", ex);
                return false;
            }
        }


        /// <summary>
        /// Check is a path is a valid repository
        /// </summary>
        /// <param name="respositoryPath"></param>
        /// <returns></returns>
        public bool IsValid(string respositoryPath)
        {
            return Repository.IsValid(respositoryPath);
        }

        public void VerifyRepository(string sourceUrl, string repositoryFolder)
        {
            if (!Repository.IsValid(repositoryFolder))
            {
                //var cloneOpt = new CloneOptions
                //{
                //    CredentialsProvider = (ur, us, ce) => new UsernamePasswordCredentials
                //    {
                //        Username = "MyUser",
                //        Password = ("Some App password from bitbucket")
                //    },
                //    OnTransferProgress = onTransfer,
                //    CertificateCheck = onCertCheck,
                //    OnProgress = onProgress,
                //};

                try
                {
                    StaticObjects.Logger.Info($"Start cloning repository: {sourceUrl}");
                    StaticObjects.Logger.Info($" Destination folder: {repositoryFolder}");
                    RepoPath = Repository.Clone(sourceUrl, repositoryFolder);
                    StaticObjects.Logger.Info($" Cloned to: {RepoPath}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }

            }
            else
            {
                RepoPath = Repository.Init(repositoryFolder);
            }
        }

        public bool Pull(string repositoryPath)
        {
            try
            {
                using Repository localRepo = new Repository(repositoryPath);
                PullOptions pullOptions = new PullOptions();
                pullOptions.FetchOptions = new FetchOptions();
                //options.CredentialsProvider = Credentials(string username, string password)
                Commands.Pull(localRepo, new Signature("rogreis", "rogreis@gmail.com", new DateTimeOffset(DateTime.Now)), pullOptions);
                return true;
            }
            catch (Exception ex)
            {
                LibraryEventsControl.FireSendUserAndLogMessage($"Pull Error, repository= {repositoryPath}: ", ex);
                LibraryEventsControl.FireSendUserAndLogMessage($"Check your internet connection");
                return true; // Pocess continues, pull errors  are ignored
            }
        }

        private static void CheckoutProgress(string path, int completedSteps, int totalSteps)
        {
            LibraryEventsControl.FireSendMessage($"Checkout progress: {completedSteps} of {totalSteps}");
        }

        /// <summary>
        /// Checkout a branch overwriting local changes
        /// </summary>
        /// <param name="repositoryPath"></param>
        /// <param name="branchName"></param>
        /// <returns></returns>
        public bool Checkout(string repositoryPath, string branchName)
        {
            try
            {
                using Repository localRepo = new Repository(repositoryPath);

                Branch branch = localRepo.Branches.ToList().Find(b => b.CanonicalName == branchName);
                if (branch == null)
                {
                    // Let's get a reference on the remote tracking branch...
                    string trackedBranchName = $"origin/{branchName}";
                    Branch trackedBranch = localRepo.Branches[trackedBranchName];

                    // ...and create a local branch pointing at the same Commit
                    branch = localRepo.CreateBranch(branchName, trackedBranch.Tip);

                    // The local branch is not configured to track anything
                    if (!branch.IsTracking)
                    {
                        // So, let's configure the local branch to track the remote one.
                        Branch updatedBranch = localRepo.Branches.Update(branch, b => b.TrackedBranch = trackedBranch.CanonicalName);
                    }

                }
                CheckoutOptions options = new CheckoutOptions() { CheckoutModifiers = CheckoutModifiers.Force, OnCheckoutProgress = CheckoutProgress };
                Branch currentBranch = Commands.Checkout(localRepo, branchName, options);
                return true;
            }
            catch (Exception ex)
            {
                LibraryEventsControl.FireSendUserAndLogMessage($"Checkout Error, repository= {repositoryPath}, branch= {branchName}: ", ex);
                return false;
            }
        }


        public bool Clone(string sourceUrl, string repositoryPath)
        {
            try
            {
                var cloneOptions = new CloneOptions { BranchName = "master", Checkout = true };
                var cloneResult = Repository.Clone(sourceUrl, repositoryPath);
                return true;
            }
            catch (Exception ex)
            {
                LibraryEventsControl.FireSendUserAndLogMessage($"Clone Error, repository= {repositoryPath}, sourceUrl= {sourceUrl}: ", ex);
                return false;
            }
        }

        //public void Test(string repositoryPath, string branchName, string url)
        //{
        //    Directory.CreateDirectory(repositoryPath);
        //    if (!Repository.IsValid(repositoryPath))
        //    {
        //        Clone(url, repositoryPath);
        //    }

        //    using Repository localRepo = new Repository(repositoryPath);

        //    Branch branch = localRepo.Branches.ToList().Find(b => b.CanonicalName == branchName);

        //    if (branch == null)
        //    {
        //        // Let's get a reference on the remote tracking branch...
        //        string trackedBranchName = $"origin/{branchName}";
        //        Branch trackedBranch = localRepo.Branches[trackedBranchName];

        //        // ...and create a local branch pointing at the same Commit
        //        branch = localRepo.CreateBranch(branchName, trackedBranch.Tip);

        //        // The local branch is not configured to track anything
        //        if (branch.IsTracking)
        //        {
        //            return;
        //        }

        //        // So, let's configure the local branch to track the remote one.
        //        Branch updatedBranch = localRepo.Branches.Update(branch, b => b.TrackedBranch = trackedBranch.CanonicalName);
        //    }

        //    CheckoutOptions options = new CheckoutOptions() { CheckoutModifiers = CheckoutModifiers.Force, OnCheckoutProgress = CheckoutProgress };
        //    Commands.Checkout(localRepo, branchName, options);


        //    //Process.Start


        //    //var remoteBranches = localRepo.Network.Remotes
        //    //                    .SelectMany(r => localRepo.Branches.Where(b =>
        //    //                        b.IsRemote &&
        //    //                        b.CanonicalName == $"refs/remotes/{r.Name}/{branchName}"))
        //    //                    .ToList();
        //    //Commands.Checkout(localRepo, remoteBranches[0].Tip.Tree, options, remoteBranches[0].Tip.Tree.Sha);
        //}

        ///// <summary>
        ///// Push the repository to the remote
        ///// </summary>
        ///// <param name="repositoryPath"></param>
        ///// <param name="username"></param>
        ///// <param name="password"></param>
        ///// <returns></returns>
        //public bool Push(string repositoryPath, string username, string password, string email, string branch)
        //{
        //    try
        //    {
        //        using (var repo = new Repository(repositoryPath))
        //        {

        //            var remote = repo.Network.Remotes[branch];
        //            string gitUser = "rogreis@gmail.com", gitToken = "720685f529537d57e95b159e48c0fb631c1c8ba0";
        //            PushOptions pushOptions = new PushOptions
        //            {
        //                CredentialsProvider = (_url, _user, _cred) =>
        //                    new UsernamePasswordCredentials { Username = gitUser, Password = gitToken }
        //            };


        //            pushOptions.CredentialsProvider = new CredentialsHandler(
        //              (url, usernameFromUrl, types) => new UsernamePasswordCredentials()
        //              {
        //                  Username = gitUser,
        //                  Password = gitToken
        //              });

        //            //var pushRefSpec = @"refs/heads/master";
        //            //repo.Network.Push(remote, pushRefSpec, pushOptions, new Signature(username, email, DateTimeOffset.Now),
        //            //    "pushed changes");

        //            //// Fetch the remote repository to ensure it's up-to-date
        //            //var refSpecs = remote.FetchRefSpecs.Select(x => x.Specification);
        //            //Commands.Fetch(repo, remote.Name, refSpecs, new FetchOptions(), "");


        //            //// Replace "sshUsername" and "sshKeyFilePath" with your SSH credentials
        //            //var sshKeyFile = "720685f529537d57e95b159e48c0fb631c1c8ba0";
        //            //var sshUsername = "sshUsername";
        //            //var sshAuth = new PrivateKeyAuthentication(sshUsername, sshKeyFile);

        //            //CloneOptions co = new CloneOptions();
        //            //co.CredentialsProvider = (_url, _user, _cred) => new UsernamePasswordCredentials { Username = gitUser, Password = gitToken };

        //            //var options = new PushOptions
        //            //{
        //            //    CredentialsProvider = new UsernamePasswordCredentials { Username = gitUser, Password = gitToken }
        //            //};


        //            // Push the changes to the remote repository
        //            string pushRefSpec = $@"refs/heads/{branch}";
        //            Branch branchFullName = repo.Branches[pushRefSpec];
        //            //repo.Network.Push(branchFullName, pushOptions);
        //            repo.Network.Push(branchFullName, pushOptions);

        //        }
        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        EventsControl.FireSendUserAndLogMessage($"Push Error, repository= {repositoryPath}, branch= {branch}: ", ex);
        //        StaticObjects.Logger.Error("Push error", ex);
        //        return false;
        //    }
        //}

        //public void Push2()
        //{
        //    // Replace "userName" with your Github username, "repoName" with the name of your repository,
        //    // and "token" with your Github API token
        //    var userName = "userName";
        //    var repoName = "repoName";
        //    var token = "token";

        //    // Replace "branchName" with the name of the branch you want to push to
        //    var branchName = "branchName";

        //    // Replace "commitMessage" with the commit message
        //    var commitMessage = "commitMessage";

        //    // Replace "fileName" with the name of the file you want to push
        //    var fileName = "fileName";

        //    // Replace "fileContent" with the content of the file
        //    var fileContent = "fileContent";

        //    // Set the Github API endpoint for pushing a file
        //    var apiUrl = $"https://api.github.com/repos/{userName}/{repoName}/contents/{fileName}";

        //    // Use the HttpClient class to send a request to the Github API
        //    using (var httpClient = new HttpClient())
        //    {
        //        // Set the Authorization header for the Github API request
        //        httpClient.DefaultRequestHeaders.Authorization =
        //            new System.Net.Http.Headers.AuthenticationHeaderValue("Token", token);

        //        // Get the latest commit SHA for the branch
        //        var branchUrl = $"https://api.github.com/repos/{userName}/{repoName}/git/refs/heads/{branchName}";
        //        var branchResponse = httpClient.GetAsync(branchUrl).Result;
        //        var branchJson = branchResponse.Content.ReadAsStringAsync().Result;
        //        var branch = JsonConvert.DeserializeObject<Branch>(branchJson);

        //        // Build the request body for pushing the file
        //        var requestBody = new PushRequestBody
        //        {
        //            Message = commitMessage,
        //            Sha = branch.Object.Sha,
        //            Branch = branchName,
        //            Content = Convert.ToBase64String(Encoding.UTF8.GetBytes(fileContent))
        //        };

        //        // Serialize the request body to a JSON string
        //        var requestJson = JsonConvert.SerializeObject(requestBody);

        //        // Send the PUT request to the Github API to push the file
        //        var response = httpClient.PutAsync(apiUrl, new StringContent(requestJson, Encoding.UTF8, "application/json")).Result;

        //        // Check if the request was successful
        //        if (response.IsSuccessStatusCode)
        //        {
        //            Console.WriteLine("Push successful");
        //        }
        //        else
        //        {
        //            Console.WriteLine("Push failed");
        //        }
        //    }
        //}

        //// Class to represent the branch information
        //class Branch
        //{
        //    public Object Object { get; set; }
        //}

        //// Class to represent the commit information
        //class Object
        //{
        //}


    }
}



﻿namespace Sierra.Common
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.TeamFoundation.Core.WebApi;
    using Microsoft.TeamFoundation.SourceControl.WebApi;

    /// <summary>
    /// Contains Sierra extensions methods to the <see cref="GitHttpClient"/> part of the VSTS SDK.
    /// </summary>
    public static class GitHttpClientExtensions
    {
        /// <summary>
        /// Creates a repository Fork through the VSTS <see cref="GitHttpClient"/>.
        /// </summary>
        /// <param name="client">The <see cref="GitHttpClient"/> used to create the Fork.</param>
        /// <param name="vstsCollectionId">The target collection ID where we are creating the fork on.</param>
        /// <param name="vstsTargetProjectId">The target project ID where we are creating the fork on.</param>
        /// <param name="sourceRepo">The origin repo for the Fork.</param>
        /// <param name="forkSuffix">The fork suffix that we want to give to the Fork name.</param>
        /// <returns>The async <see cref="Task"/> wrapper.</returns>
        /// <remarks>
        /// TODO: Currently missing the IDEMPOTENCY check -> Rename to CreateForkIfNotExists afterwards
        /// </remarks>
        internal static async Task CreateFork(this GitHttpClient client, string vstsCollectionId, string vstsTargetProjectId, GitRepository sourceRepo, string forkSuffix)
        {
            await client.CreateRepositoryAsync(
                new GitRepositoryCreateOptions
                {
                    Name = $"{sourceRepo.Name}-{forkSuffix}",
                    ProjectReference = new TeamProjectReference {Id = Guid.Parse(vstsTargetProjectId)},
                    ParentRepository = new GitRepositoryRef
                    {
                        Id = sourceRepo.Id,
                        ProjectReference = new TeamProjectReference {Id = Guid.Parse(vstsTargetProjectId)},
                        Collection = new TeamProjectCollectionReference {Id = Guid.Parse(vstsCollectionId)}
                    }
                });
        }
    }
}
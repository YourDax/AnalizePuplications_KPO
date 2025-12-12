using AnalyzePublicationsContracts.BindingModels;
using AnalyzePublicationsContracts.SearchModels;
using AnalyzePublicationsContracts.StorageContracts;
using AnalyzePublicationsContracts.ViewModels;
using AnalyzePublicationsDatabaseImplement.Models;
using Microsoft.EntityFrameworkCore;

namespace AnalyzePublicationsDatabaseImplement.Implements
{
    public class AuthorStorage : IAuthorStorage
    {
        public List<AuthorViewModel> GetFullList()
        {
            using var context = new PublicationsDatabase();
            return context.Authors
                .Select(x => x.GetViewModel)
                .ToList();
        }

        public List<AuthorViewModel> GetFilteredList(AuthorSearchModel model)
        {
            if (model == null) return GetFullList();

            using var context = new PublicationsDatabase();
            var query = context.Authors.AsQueryable();

            if (!string.IsNullOrEmpty(model.FullName))
                query = query.Where(x => x.FullName.Contains(model.FullName));
            if (!string.IsNullOrEmpty(model.Affiliation))
                query = query.Where(x => x.Affiliation.Contains(model.Affiliation));

            return query.Select(x => x.GetViewModel).ToList();
        }

        public AuthorViewModel? GetElement(AuthorSearchModel model)
        {
            if (model == null) return null;

            using var context = new PublicationsDatabase();

            if (!string.IsNullOrEmpty(model.FullName))
            {
                return context.Authors.FirstOrDefault(x => x.FullName == model.FullName)?.GetViewModel;
            }

            if (model.Id.HasValue)
            {
                return context.Authors.FirstOrDefault(x => x.Id == model.Id.Value)?.GetViewModel;
            }

            return null;
        }

        public AuthorViewModel? Insert(AuthorBindingModel model)
        {
            if (model == null) return null;

            try
            {
                using var context = new PublicationsDatabase();
                var entity = Author.Create(model);
                if (entity == null) return null;

                context.Authors.Add(entity);
                context.SaveChanges();
                return entity.GetViewModel;
            }
            catch
            {
                return null;
            }
        }

        public AuthorViewModel? Update(AuthorBindingModel model)
        {
            if (model == null) return null;

            try
            {
                using var context = new PublicationsDatabase();
                var entity = context.Authors.FirstOrDefault(x => x.Id == model.Id);
                if (entity == null) return null;

                entity.Update(model);
                context.SaveChanges();
                return entity.GetViewModel;
            }
            catch
            {
                return null;
            }
        }

        public AuthorViewModel? Delete(AuthorBindingModel model)
        {
            if (model == null) return null;

            try
            {
                using var context = new PublicationsDatabase();
                var entity = context.Authors.FirstOrDefault(x => x.Id == model.Id);
                if (entity == null) return null;

                context.Authors.Remove(entity);
                context.SaveChanges();
                return entity.GetViewModel;
            }
            catch
            {
                return null;
            }
        }
    }
}

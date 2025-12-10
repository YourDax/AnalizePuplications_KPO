using AnalyzePublicationsContracts.BindingModels;
using AnalyzePublicationsContracts.SearchModels;
using AnalyzePublicationsContracts.StorageContracts;
using AnalyzePublicationsContracts.ViewModels;
using AnalyzePublicationsDatabaseImplement.Models;
using Microsoft.EntityFrameworkCore;

namespace AnalyzePublicationsDatabaseImplement.Implements
{
    public class PublicationStorage : IPublicationStorage
    {
        public List<PublicationViewModel> GetFullList()
        {
            using var context = new PublicationsDatabase();
            return context.Publications
                .Include(x => x.Source)
                .Select(x => x.GetViewModel)
                .ToList();
        }

        public List<PublicationViewModel> GetFilteredList(PublicationSearchModel model)
        {
            using var context = new PublicationsDatabase();
            var query = context.Publications.Include(x => x.Source).AsQueryable();

            if (!string.IsNullOrEmpty(model.Title))
                query = query.Where(x => x.Title.Contains(model.Title));
            if (model.Year.HasValue)
                query = query.Where(x => x.Year == model.Year.Value);
            if (model.SourceId.HasValue)
                query = query.Where(x => x.SourceId == model.SourceId.Value);

            return query.Select(x => x.GetViewModel).ToList();
        }

        public PublicationViewModel? GetElement(PublicationSearchModel model)
        {
            using var context = new PublicationsDatabase();

            if (!string.IsNullOrEmpty(model.Title))
                return context.Publications.Include(x => x.Source)
                    .FirstOrDefault(x => x.Title == model.Title)?.GetViewModel;

            if (model.Id.HasValue)
                return context.Publications.Include(x => x.Source)
                    .FirstOrDefault(x => x.Id == model.Id.Value)?.GetViewModel;

            return null;
        }

        public PublicationViewModel? Insert(PublicationBindingModel model)
        {
            using var context = new PublicationsDatabase();
            var entity = Publication.Create(model);
            context.Publications.Add(entity);
            context.SaveChanges();
            return entity.GetViewModel;
        }

        public PublicationViewModel? Update(PublicationBindingModel model)
        {
            using var context = new PublicationsDatabase();
            var entity = context.Publications.FirstOrDefault(x => x.Id == model.Id);
            if (entity == null) return null;

            entity.Update(model);
            context.SaveChanges();
            return entity.GetViewModel;
        }

        public PublicationViewModel? Delete(PublicationBindingModel model)
        {
            using var context = new PublicationsDatabase();
            var entity = context.Publications.FirstOrDefault(x => x.Id == model.Id);
            if (entity == null) return null;

            context.Publications.Remove(entity);
            context.SaveChanges();
            return entity.GetViewModel;
        }
    }
}

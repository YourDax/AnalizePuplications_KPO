using AnalyzePublicationsContracts.BindingModels;
using AnalyzePublicationsContracts.SearchModels;
using AnalyzePublicationsContracts.StorageContracts;
using AnalyzePublicationsContracts.ViewModels;
using AnalyzePublicationsDatabaseImplement.Models;
using Microsoft.EntityFrameworkCore;

namespace AnalyzePublicationsDatabaseImplement.Implements
{
    public class SourceStorage : ISourceStorage
    {
        public List<SourceViewModel> GetFullList()
        {
            using var context = new PublicationsDatabase();
            return context.Sources.Select(x => x.GetViewModel).ToList();
        }

        public List<SourceViewModel> GetFilteredList(SourceSearchModel model)
        {
            using var context = new PublicationsDatabase();
            var query = context.Sources.AsQueryable();

            if (!string.IsNullOrEmpty(model.Name))
                query = query.Where(x => x.Name.Contains(model.Name));

            return query.Select(x => x.GetViewModel).ToList();
        }

        public SourceViewModel? GetElement(SourceSearchModel model)
        {
            using var context = new PublicationsDatabase();
            if (!string.IsNullOrEmpty(model.Name))
                return context.Sources.FirstOrDefault(x => x.Name == model.Name)?.GetViewModel;
            if (model.Id.HasValue)
                return context.Sources.FirstOrDefault(x => x.Id == model.Id.Value)?.GetViewModel;
            return null;
        }

        public SourceViewModel? Insert(SourceBindingModel model)
        {
            using var context = new PublicationsDatabase();
            var entity = Source.Create(model);
            context.Sources.Add(entity);
            context.SaveChanges();
            return entity.GetViewModel;
        }

        public SourceViewModel? Update(SourceBindingModel model)
        {
            using var context = new PublicationsDatabase();
            var entity = context.Sources.FirstOrDefault(x => x.Id == model.Id);
            if (entity == null) return null;

            entity.Update(model);
            context.SaveChanges();
            return entity.GetViewModel;
        }

        public SourceViewModel? Delete(SourceBindingModel model)
        {
            using var context = new PublicationsDatabase();
            var entity = context.Sources.FirstOrDefault(x => x.Id == model.Id);
            if (entity == null) return null;

            context.Sources.Remove(entity);
            context.SaveChanges();
            return entity.GetViewModel;
        }
    }
}

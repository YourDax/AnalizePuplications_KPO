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

            var query = context.Publications
                .Include(p => p.Source)
                .Include(p => p.PublicationAuthors) // подгружаем связи с авторами
                .AsQueryable();

            if (!string.IsNullOrEmpty(model.Title))
                query = query.Where(x => x.Title.Contains(model.Title));
            if (model.Year.HasValue)
                query = query.Where(x => x.Year == model.Year.Value);
            if (model.SourceId.HasValue)
                query = query.Where(x => x.SourceId == model.SourceId.Value);

            return query
                .Select(x => new PublicationViewModel
                {
                    Id = x.Id,
                    Title = x.Title,
                    Year = x.Year,
                    PublicationType = x.PublicationType,
                    SourceId = x.SourceId,
                    Doi = x.Doi,
                    CitationCount = x.CitationCount,
                    AuthorIds = x.PublicationAuthors.Select(pa => pa.AuthorId).ToList()
                })
                .ToList();
        }

        public PublicationViewModel? GetElement(PublicationSearchModel model)
        {
            using var context = new PublicationsDatabase();

            var query = context.Publications
                .Include(p => p.Source)
                .Include(p => p.PublicationAuthors)
                .AsQueryable();

            Publication? entity = null;

            if (!string.IsNullOrEmpty(model.Title))
                entity = query.FirstOrDefault(x => x.Title == model.Title);

            if (model.Id.HasValue)
                entity = query.FirstOrDefault(x => x.Id == model.Id.Value);

            if (entity == null) return null;

            return new PublicationViewModel
            {
                Id = entity.Id,
                Title = entity.Title,
                Year = entity.Year,
                PublicationType = entity.PublicationType,
                SourceId = entity.SourceId,
                Doi = entity.Doi,
                CitationCount = entity.CitationCount,
                AuthorIds = entity.PublicationAuthors.Select(pa => pa.AuthorId).ToList()
            };
        }


        public PublicationViewModel? Insert(PublicationBindingModel model)
        {
            using var context = new PublicationsDatabase();
            var entity = Publication.Create(model);
            context.Publications.Add(entity);
            context.SaveChanges();

            // создаём связи с авторами
            if (model.AuthorIds != null)
            {
                foreach (var authorId in model.AuthorIds)
                {
                    context.PublicationAuthors.Add(new PublicationAuthor
                    {
                        PublicationId = entity.Id,
                        AuthorId = authorId
                    });
                }
                context.SaveChanges();
            }

            return entity.GetViewModel;
        }


        public PublicationViewModel? Update(PublicationBindingModel model)
        {
            using var context = new PublicationsDatabase();
            var entity = context.Publications
                .Include(p => p.PublicationAuthors)
                .FirstOrDefault(x => x.Id == model.Id);
            if (entity == null) return null;

            entity.Update(model);

            // удаляем старые связи
            context.PublicationAuthors.RemoveRange(entity.PublicationAuthors);
            context.SaveChanges();

            // добавляем новые связи
            if (model.AuthorIds != null)
            {
                foreach (var authorId in model.AuthorIds)
                {
                    context.PublicationAuthors.Add(new PublicationAuthor
                    {
                        PublicationId = entity.Id,
                        AuthorId = authorId
                    });
                }
            }

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

        public List<PublicationViewModel> GetPublicationsByAuthor(int authorId)
        {
            using var context = new PublicationsDatabase();

            return context.Publications
                .Include(p => p.Source)
                .Include(p => p.PublicationAuthors)
                .Where(p => p.PublicationAuthors
                    .Any(pa => pa.AuthorId == authorId))
                .Select(p => p.GetViewModel)
                .ToList();
        }

    }
}

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
                .Include(a => a.PublicationAuthors)
                    .ThenInclude(pa => pa.Publication)
                .ToList()
                .Select(a => CalculateMetricsAndCreateViewModel(context, a))
                .ToList();
        }

        public List<AuthorViewModel> GetFilteredList(AuthorSearchModel model)
        {
            if (model == null) return GetFullList();

            using var context = new PublicationsDatabase();
            var query = context.Authors
                .Include(a => a.PublicationAuthors)
                    .ThenInclude(pa => pa.Publication)
                .AsQueryable();

            if (!string.IsNullOrEmpty(model.FullName))
                query = query.Where(x => x.FullName.Contains(model.FullName));
            if (!string.IsNullOrEmpty(model.Affiliation))
                query = query.Where(x => x.Affiliation.Contains(model.Affiliation));

            return query.ToList()
                .Select(a => CalculateMetricsAndCreateViewModel(context, a))
                .ToList();
        }

        public AuthorViewModel? GetElement(AuthorSearchModel model)
        {
            if (model == null) return null;

            using var context = new PublicationsDatabase();
            Author? entity = null;

            if (!string.IsNullOrEmpty(model.FullName))
            {
                entity = context.Authors
                    .Include(a => a.PublicationAuthors)
                        .ThenInclude(pa => pa.Publication)
                    .FirstOrDefault(x => x.FullName == model.FullName);
            }
            else if (model.Id.HasValue)
            {
                entity = context.Authors
                    .Include(a => a.PublicationAuthors)
                        .ThenInclude(pa => pa.Publication)
                    .FirstOrDefault(x => x.Id == model.Id.Value);
            }

            if (entity == null) return null;

            return CalculateMetricsAndCreateViewModel(context, entity);
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

                // Перезагружаем с навигационными свойствами для вычисления метрик
                entity = context.Authors
                    .Include(a => a.PublicationAuthors)
                        .ThenInclude(pa => pa.Publication)
                    .FirstOrDefault(a => a.Id == entity.Id);

                if (entity == null) return null;

                return CalculateMetricsAndCreateViewModel(context, entity);
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
                var entity = context.Authors
                    .Include(a => a.PublicationAuthors)
                        .ThenInclude(pa => pa.Publication)
                    .FirstOrDefault(x => x.Id == model.Id);
                if (entity == null) return null;

                entity.Update(model);
                context.SaveChanges();

                return CalculateMetricsAndCreateViewModel(context, entity);
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
                var entity = context.Authors
                    .Include(a => a.PublicationAuthors)
                        .ThenInclude(pa => pa.Publication)
                    .FirstOrDefault(x => x.Id == model.Id);
                if (entity == null) return null;

                var viewModel = CalculateMetricsAndCreateViewModel(context, entity);
                context.Authors.Remove(entity);
                context.SaveChanges();
                return viewModel;
            }
            catch
            {
                return null;
            }
        }

        private AuthorViewModel CalculateMetricsAndCreateViewModel(PublicationsDatabase context, Author author)
        {
            var publicationIds = author.PublicationAuthors?
                .Select(pa => pa.PublicationId)
                .Distinct()
                .ToList() ?? new List<int>();

            var publicationsCount = publicationIds.Count;

            // Подсчет соавторов
            var coauthorsCount = 0;
            if (publicationIds.Any())
            {
                var allAuthorIds = context.PublicationAuthors
                    .Where(pa => publicationIds.Contains(pa.PublicationId) && pa.AuthorId != author.Id)
                    .Select(pa => pa.AuthorId)
                    .Distinct()
                    .Count();
                coauthorsCount = allAuthorIds;
            }

            // Подсчет цитирований
            var citationCount = 0;
            if (publicationIds.Any())
            {
                citationCount = context.Publications
                    .Where(p => publicationIds.Contains(p.Id))
                    .Sum(p => p.CitationCount);
            }

            // Вычисление H-индекса
            var hIndex = 0;
            if (publicationIds.Any())
            {
                var citations = context.Publications
                    .Where(p => publicationIds.Contains(p.Id))
                    .Select(p => p.CitationCount)
                    .OrderByDescending(c => c)
                    .ToList();

                for (int i = 0; i < citations.Count; i++)
                {
                    if (citations[i] >= i + 1)
                        hIndex = i + 1;
                    else
                        break;
                }
            }

            // Среднее количество страниц
            var averagePages = 0.0;
            if (publicationIds.Any())
            {
                var pagesList = context.Publications
                    .Where(p => publicationIds.Contains(p.Id) && p.Pages.HasValue)
                    .Select(p => p.Pages!.Value)
                    .ToList();

                if (pagesList.Any())
                {
                    averagePages = pagesList.Average();
                }
            }

            return author.GetViewModel(publicationsCount, coauthorsCount, citationCount, hIndex, averagePages);
        }
    }
}

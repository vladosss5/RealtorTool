using System;
using System.Linq;
using Avalonia.Media.Imaging;
using ReactiveUI.Fody.Helpers;
using RealtorTool.Core.DbEntities;

namespace RealtorTool.Desktop.ViewModels.Items;

public class ListingItemViewModel : ViewModelBase
{
    public Listing Listing { get; }
    
    [Reactive] public Bitmap? MainImage { get; set; }
    [Reactive] public bool HasImage { get; set; }

    public ListingItemViewModel(Listing listing)
    {
        Listing = listing;
        LoadMainImage();
    }

    private void LoadMainImage()
    {
        var mainPhoto = Listing.Realty?.Photos?.FirstOrDefault();
        if (mainPhoto?.FileData != null && mainPhoto.FileData.Length > 0)
        {
            try
            {
                using var stream = new System.IO.MemoryStream(mainPhoto.FileData);
                MainImage = new Bitmap(stream);
                HasImage = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка загрузки изображения: {ex.Message}");
                HasImage = false;
            }
        }
        else
        {
            HasImage = false;
        }
    }
}
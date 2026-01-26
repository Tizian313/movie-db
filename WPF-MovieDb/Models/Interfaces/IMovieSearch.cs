using MovieDB.SharedModels;
using System.Collections.ObjectModel;
using WPF_MovieDb.Models.Items;

namespace WPF_MovieDb.Models.Interfaces;


public interface IMovieSearch
{
    List<Movie> GetMovieList(string input, string searchBy, string rank, bool invert, ObservableCollection<LabeledCheckBox> genreCheckBoxes);
    List<Movie> SearchMovieNames(string input);
}

using Avalonia.Controls;

namespace RealtorTool.Services.Interfaces;

/// <summary>
/// Интерфейс для управления окнами.
/// </summary>
public interface IWindowService
{
    /// <summary>
    /// Показать окно.
    /// </summary>
    /// <typeparam name="TWindow">Открываемое окно.</typeparam>
    void ShowWindow<TWindow>() where TWindow : Window;
    
    /// <summary>
    /// Показать диалоговое окно.
    /// </summary>
    /// <param name="owner">Открывающий.</param>
    /// <typeparam name="TWindow">Открываемое окно.</typeparam>
    void ShowDialog<TWindow>(Window owner) where TWindow : Window;
    
    /// <summary>
    /// Закрыть окно.
    /// </summary>
    /// <param name="window"></param>
    void CloseWindow(Window window);
    
    /// <summary>
    /// Закрыть текущее окно.
    /// </summary>
    void CloseCurrentWindow();

    /// <summary>
    /// Записать текущее окно.
    /// </summary>
    /// <param name="window">Окно.</param>
    public void SetCurrentWindow(Window window);
}
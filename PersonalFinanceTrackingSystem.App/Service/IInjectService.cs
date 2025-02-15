namespace PersonalFinanceTrackingSystem.App.Service
{
    public interface IInjectService
    {
        Task SuccessMessage(string message);
        Task ErrorMessage(string message);
        Task<bool> ConfirmMessageBox(string message);
    }
}

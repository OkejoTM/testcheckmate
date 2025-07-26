using System.IO;
using System.Globalization;
using Domain.Enums;
using Domain.Entities;
using Infrustructure.Services;
using Shared;
using Shared.Entities;

namespace Infrustructure.Extensions;

public static class ReceiptExtensions {
    public static string ConstructCloudFilePath(this Receipt receipt, StoredFile storedFile, User user) {
        string categoryFolderPath;
        string fileName;

        DateTime receiptDate;
        if (!DateTime.TryParse(receipt.Date, out receiptDate)) {
            receiptDate = receipt.CreatedAt;
        }

        if (!(receipt.CategoryByStore == ECategoryByStore.English || receipt.CategoryByStore == ECategoryByStore.Sports) &&
            user.Role == Roles.OfficeManager)
        {
            categoryFolderPath = ReceiptFolderService.CorporateCardExpensesFolderPath;
            fileName = $"{receiptDate.ToString("MMdd")} {receipt.CategoryByStore.ToString()} {receipt.Total}{Path.GetExtension(storedFile.Path)}";
        }
        else {
            categoryFolderPath = ReceiptFolderService.EmployeeCompensationFolderPath;
            fileName = $"{receiptDate.ToString("MMdd")} {user.LastName} {receipt.CategoryByStore.ToString()}{Path.GetExtension(storedFile.Path)}";
        }

        string yearFolderPath = receiptDate.ToString("yyyy");
        string monthFolderPath = receiptDate.ToString("MM MMMM", CultureInfo.GetCultureInfo("ru"));

        return Path.Combine(categoryFolderPath, yearFolderPath, monthFolderPath, fileName);
    }
}

namespace EStoreX.Core.ServiceContracts.Common
{
    /// <summary>
    /// Service contract for exporting data into different file formats.  
    /// Provides methods to export generic collections to CSV, Excel, and PDF.
    /// </summary>
    public interface IExportService
    {
        /// <summary>
        /// Exports the given data collection to a CSV file format.
        /// </summary>
        /// <typeparam name="T">The type of objects in the collection.</typeparam>
        /// <param name="data">The data to be exported.</param>
        /// <returns>
        /// A byte array representing the generated CSV file.
        /// </returns>
        byte[] ExportToCsv<T>(IEnumerable<T> data);

        /// <summary>
        /// Exports the given data collection to an Excel file format (.xlsx).
        /// </summary>
        /// <typeparam name="T">The type of objects in the collection.</typeparam>
        /// <param name="data">The data to be exported.</param>
        /// <returns>
        /// A byte array representing the generated Excel file.
        /// </returns>
        byte[] ExportToExcel<T>(IEnumerable<T> data);

        /// <summary>
        /// Exports the given data collection to a PDF document.
        /// </summary>
        /// <typeparam name="T">The type of objects in the collection.</typeparam>
        /// <param name="data">The data to be exported.</param>
        /// <returns>
        /// A byte array representing the generated PDF file.
        /// </returns>
        byte[] ExportToPdf<T>(IEnumerable<T> data);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using iText.IO.Image;
using iText.Kernel.Colors;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using QRCoder;
using System.Drawing;
using System.Drawing.Imaging;
using BarcodeLib;

namespace SportClubApp.Services
{
    public class CarnetPDFService
    {
        private readonly string _carpetaCarnets;

        public CarnetPDFService()
        {
            _carpetaCarnets = Path.Combine(Directory.GetCurrentDirectory(), "Carnets");
            if (!Directory.Exists(_carpetaCarnets))
                Directory.CreateDirectory(_carpetaCarnets);
        }

        public string GenerarCarnet(SocioInfo socio, string logoPath = null)
        {
            try
            {
                string fileName = $"Carnet_{socio.NroSocio}_{DateTime.Now:yyyyMMddHHmmss}.pdf";
                string filePath = Path.Combine(_carpetaCarnets, fileName);

                using (var writer = new PdfWriter(filePath))
                using (var pdf = new PdfDocument(writer))
                using (var document = new Document(pdf))
                {
                    document.SetMargins(10, 10, 10, 10);

                    // ===== FILA 1: ENCABEZADO (100% ANCHO) =====
                    var headerTable = new Table(2).SetWidth(UnitValue.CreatePercentValue(100));

                    // Logo (izquierda)
                    if (!string.IsNullOrEmpty(logoPath) && File.Exists(logoPath))
                    {
                        try
                        {
                            var logoData = ImageDataFactory.Create(logoPath);
                            var logoCell = new Cell()
                                .Add(new iText.Layout.Element.Image(logoData)
                                    .SetWidth(50)
                                    .SetHeight(50))
                                .SetBorder(iText.Layout.Borders.Border.NO_BORDER)
                                .SetVerticalAlignment(VerticalAlignment.MIDDLE);
                            headerTable.AddCell(logoCell);
                        }
                        catch (Exception ex)
                        {
                            headerTable.AddCell(new Cell().Add(new Paragraph("")).SetBorder(iText.Layout.Borders.Border.NO_BORDER));
                        }
                    }
                    else
                    {
                        headerTable.AddCell(new Cell().Add(new Paragraph("")).SetBorder(iText.Layout.Borders.Border.NO_BORDER));
                    }

                    // Título (derecha)
                    var titleCell = new Cell()
                        .Add(new Paragraph("CLUB DEPORTIVO G5")
                            .SetFontSize(18)
                            .SetTextAlignment(TextAlignment.CENTER))
                        .Add(new Paragraph("CARNET DE SOCIO")
                            .SetFontSize(14)
                            .SetTextAlignment(TextAlignment.CENTER))
                        .SetBorder(iText.Layout.Borders.Border.NO_BORDER)
                        .SetVerticalAlignment(VerticalAlignment.MIDDLE);
                    headerTable.AddCell(titleCell);

                    document.Add(headerTable);

                    // ===== FILA 2: CONTENIDO PRINCIPAL (2 COLUMNAS) =====
                    var mainContentTable = new Table(new float[] { 3, 2 }) // 60% - 40%
                        .SetWidth(UnitValue.CreatePercentValue(100))
                        .SetMarginTop(10);

                    // ===== COLUMNA IZQUIERDA (60%) =====
                    var leftColumn = new Cell()
                        .SetBorder(iText.Layout.Borders.Border.NO_BORDER); // ✅ AGREGAR ESTO

                    // Tabla con Foto y Datos
                    var fotoDatosTable = new Table(2).SetWidth(UnitValue.CreatePercentValue(100));

                    // Celda de Foto
                    var fotoCell = new Cell()
                        .SetBorder(iText.Layout.Borders.Border.NO_BORDER); // ✅ AGREGAR ESTO

                    if (socio.FotoCarnet != null && socio.FotoCarnet.Length > 0)
                    {
                        try
                        {
                            var fotoData = ImageDataFactory.Create(socio.FotoCarnet);
                            var fotoImage = new iText.Layout.Element.Image(fotoData)
                                .SetWidth(80)
                                .SetHeight(100);
                            fotoCell.Add(fotoImage.SetTextAlignment(TextAlignment.CENTER));
                        }
                        catch (Exception ex)
                        {
                            fotoCell.Add(new Paragraph("[SIN FOTO]")
                                .SetFontSize(9)
                                .SetTextAlignment(TextAlignment.CENTER));
                        }
                    }
                    else
                    {
                        fotoCell.Add(new Paragraph("[SIN FOTO]")
                            .SetFontSize(9)
                            .SetTextAlignment(TextAlignment.CENTER));
                    }
                    fotoCell.SetVerticalAlignment(VerticalAlignment.MIDDLE)
                           .SetTextAlignment(TextAlignment.CENTER)
                           .SetPadding(5);
                    fotoDatosTable.AddCell(fotoCell);

                    // Celda de Datos
                    var datosCell = new Cell()
                        .SetBorder(iText.Layout.Borders.Border.NO_BORDER) // ✅ AGREGAR ESTO
                        .Add(new Paragraph($"N° Socio: {socio.NroSocio}")
                            .SetFontSize(11)
                            .SetMarginBottom(6))
                        .Add(new Paragraph($"Nombre: {socio.NombreCompleto}")
                            .SetFontSize(11)
                            .SetMarginBottom(6))
                        .Add(new Paragraph($"DNI: {socio.DNI}")
                            .SetFontSize(11)
                            .SetMarginBottom(6))
                        .Add(new Paragraph($"Estado: {socio.EstadoPago}")
                            .SetFontSize(11))
                        .SetPadding(10);
                    fotoDatosTable.AddCell(datosCell);

                    // ✅ AGREGAR NO_BORDER A LA TABLA FOTO-DATOS
                    fotoDatosTable.SetBorder(iText.Layout.Borders.Border.NO_BORDER);

                    leftColumn.Add(fotoDatosTable);

                    // Fecha de Emisión
                    leftColumn.Add(new Paragraph($"Emitido: {DateTime.Now:dd/MM/yyyy}")
                        .SetFontSize(10)
                        .SetTextAlignment(TextAlignment.CENTER)
                        .SetMarginTop(10));

                    // Pie de página
                    leftColumn.Add(new Paragraph("• Este carnet es personal e intransferible •")
                        .SetFontSize(8)
                        .SetTextAlignment(TextAlignment.CENTER)
                        .SetMarginTop(5));

                    mainContentTable.AddCell(leftColumn);

                    // ===== COLUMNA DERECHA (40%) =====
                    var rightColumn = new Cell()
                        .SetBorder(iText.Layout.Borders.Border.NO_BORDER) // ✅ AGREGAR ESTO
                        .SetTextAlignment(TextAlignment.CENTER)
                        .SetVerticalAlignment(VerticalAlignment.MIDDLE);

                    // QR Code
                    rightColumn.Add(new Paragraph("CÓDIGO DE VERIFICACIÓN")
                        .SetFontSize(10)
                        .SetTextAlignment(TextAlignment.CENTER)
                        .SetMarginBottom(5));

                    var qrData = GenerarDatosQR(socio);
                    var qrImageBitmap = GenerarQRCode(qrData);

                    if (qrImageBitmap != null)
                    {
                        try
                        {
                            using (var qrStream = new MemoryStream())
                            {
                                qrImageBitmap.Save(qrStream, ImageFormat.Png);
                                var qrDataPdf = ImageDataFactory.Create(qrStream.ToArray());
                                var qrImage = new iText.Layout.Element.Image(qrDataPdf)
                                                .SetWidth(100)
                                                .SetHeight(100)
                                                .SetHorizontalAlignment(iText.Layout.Properties.HorizontalAlignment.CENTER);

                                rightColumn.Add(qrImage);
                            }
                        }
                        catch (Exception qrEx)
                        {
                            Console.WriteLine($"Error generando QR: {qrEx.Message}");
                        }
                    }

                    // Código
                    string codigoSocio = string.IsNullOrEmpty(socio.CarnetCodigo) ?
                        $"SOC-{socio.NroSocio:00000}" : socio.CarnetCodigo;

                    rightColumn.Add(new Paragraph($"Código: {codigoSocio}")
                        .SetFontSize(9)
                        .SetTextAlignment(TextAlignment.CENTER)
                        .SetMarginTop(5));

                    // Código de Barras
                    var barcodeImage = GenerarCodigoBarras(codigoSocio);
                    if (barcodeImage != null)
                    {
                        try
                        {
                            using (var barcodeStream = new MemoryStream())
                            {
                                barcodeImage.Save(barcodeStream, ImageFormat.Png);
                                var barcodeData = ImageDataFactory.Create(barcodeStream.ToArray());
                                var barcodePdf = new iText.Layout.Element.Image(barcodeData)
                                    .SetWidth(150)
                                    .SetHeight(50)
                                    .SetHorizontalAlignment(iText.Layout.Properties.HorizontalAlignment.CENTER);

                                rightColumn.Add(barcodePdf.SetMarginTop(15));
                            }
                        }
                        catch (Exception bcEx)
                        {
                            Console.WriteLine($"Error generando código de barras: {bcEx.Message}");
                        }
                    }

                    mainContentTable.AddCell(rightColumn);

                    // ✅ AGREGAR NO_BORDER A LA TABLA PRINCIPAL
                    mainContentTable.SetBorder(iText.Layout.Borders.Border.NO_BORDER);

                    document.Add(mainContentTable);
                }

                return filePath;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al generar carnet PDF: {ex.Message}", ex);
            }
        }

        private void AddDataRow(Table table, string label, string value)
        {
            var labelCell = new Cell()
                .Add(new Paragraph(label).SetFontSize(11))
                .SetPadding(3)
                .SetBackgroundColor(new DeviceRgb(240, 240, 240));

            var valueCell = new Cell()
                .Add(new Paragraph(value).SetFontSize(11))
                .SetPadding(3);

            table.AddCell(labelCell);
            table.AddCell(valueCell);
        }

        private string GenerarDatosQR(SocioInfo socio)
        {
            return $"CLUB_DEPORTIVO_G5|SOCIO:{socio.NroSocio}|DNI:{socio.DNI}|NOMBRE:{socio.NombreCompleto}|COD:{socio.CarnetCodigo}|FECHA:{DateTime.Now:yyyyMMdd}";
        }

        private System.Drawing.Bitmap GenerarQRCode(string data)
        {
            try
            {
                using (var qrGenerator = new QRCodeGenerator())
                using (var qrCodeData = qrGenerator.CreateQrCode(data, QRCodeGenerator.ECCLevel.Q))
                using (var qrCode = new QRCode(qrCodeData))
                {
                    return qrCode.GetGraphic(
                        pixelsPerModule: 10,
                        darkColor: System.Drawing.Color.Black,
                        lightColor: System.Drawing.Color.White,
                        drawQuietZones: true
                    );
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en GenerarQRCode: {ex.Message}");
                return null;
            }
        }

        // ===== AGREGA ESTE MÉTODO =====
        private System.Drawing.Bitmap GenerarCodigoBarras(string codigo)
        {
            try
            {
                BarcodeLib.Barcode barcode = new BarcodeLib.Barcode();
                System.Drawing.Image barcodeImage = barcode.Encode(
                    BarcodeLib.TYPE.CODE128,
                    codigo,
                    System.Drawing.Color.Black,
                    System.Drawing.Color.White,
                    250, 70
                );
                return (System.Drawing.Bitmap)barcodeImage;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error generando código de barras: {ex.Message}");
                return null;
            }
        }
        // ===== FIN DEL MÉTODO AGREGADO =====
        public string ObtenerRutaCarpetaCarnets()
        {
            return _carpetaCarnets;
        }

        public List<string> ObtenerCarnetsGenerados()
        {
            try
            {
                if (Directory.Exists(_carpetaCarnets))
                {
                    return Directory.GetFiles(_carpetaCarnets, "Carnet_*.pdf")
                                   .OrderByDescending(f => File.GetCreationTime(f))
                                   .ToList();
                }
                return new List<string>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error obteniendo lista de carnets: {ex.Message}");
                return new List<string>();
            }
        }

        public void AbrirCarpetaCarnets()
        {
            try
            {
                if (Directory.Exists(_carpetaCarnets))
                {
                    System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                    {
                        FileName = _carpetaCarnets,
                        UseShellExecute = true
                    });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error abriendo carpeta de carnets: {ex.Message}");
            }
        }
    }

    public class SocioInfo
    {
        public int NroSocio { get; set; }
        public string NombreCompleto { get; set; }
        public string DNI { get; set; }
        public string EstadoPago { get; set; }
        public byte[] FotoCarnet { get; set; }
        public string CarnetCodigo { get; set; }
        public DateTime? FechaEmision { get; set; }
    }
}
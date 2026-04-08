using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using QuestPDF.Companion; // Opcional - reemplaza al antiguo Previewer

namespace PdfCreate.Service
{
    public class ReporteService
    {
        public byte[] GenerarPdfDinamico(List<Producto> productos, string color)
        {
            var datosAuto = new Dictionary<string, string> {
    { "Versión:", "SENTRA SR PLATINUM BITONO CVT 26" },
    { "Año:", "2026" },
    { "Precio:", "$408,900.00" }
};
            var datosPlan = new Dictionary<string, string> {
    { "Nombre de la Oferta:", "CRÉDITO NISSAN SEGURO PROMOCION SENTRA Y VERSA 2026 ENG MAYOR A 35% MAR26" },
    { "Enganche:", "$540,890.00 (10.00%)" },
    { "Monto a Financiar:", "$397,985.78" },
    { "Pago Inicial:", "$62,490.19" },
    { "Mensualidad a partir del mes 13:", "$2,490.19" },
    { "Tasa Anual:", "13.99%" },
    { "Personalidad Fiscal:", "PF" },
    { "Comisión por Apertura:", "$2,690.69 (2.50%)" },
    { "Tipo Comisión por Apertura:", "FINANCIADO" }
};
            var datosSeguro = new Dictionary<string, string> {
    { "Estado:", "Tabasco" },
    { "Municipio:", "Jalpa de mendez" },
    { "Tipo:", "FINANCIADO" },
    { "Plazo:", "24 Meses" },
    { "Aseguradora:", "GNP" },
    { "Monto Primer Recibo:", "$5,000.52" },
    { "Monto Subsecuente:", "$15,162.82" },
    { "Tipo de paquete:", "4552 - AUTOS NISSAN PROMOCION SUB" },
};
            var datosGarantia = new Dictionary<string, string> {
    { "Financiado:", "NISSAN GARANTIA EXTENDIDA 2 AÑOS" },
    { "Monto a Financiar:", "$97,985.78" },
};

            var datosAdicionales = new Dictionary<string, string> {
    { "Servicio Total Nissan 1 año PROTECCIÓN:", "$18,362.78" },
    { "prevencion de robos:", "$17,563.78" },
    { "Monto a Financiar:", "$13,432.78" },
};



            var documento = Document.Create(container =>
            {
                container.Page(page =>
                {
                    // Configuración de la página
                    page.Size(PageSizes.A4);
                    page.Margin(1, Unit.Centimetre);
                    page.PageColor(Colors.White);
                    page.DefaultTextStyle(x => x.FontSize(10));

                    // --- ENCABEZADO (Se repite en cada página) ---
                    page.Header().Element(element => ComposeHeader(element, color));


                    page.Content().PaddingVertical(10).Column(mainCol =>
                    {
                        mainCol.Item().Image("Assets/front_publicidad.png");
                        // --- FILA PRINCIPAL: 2 COLUMNAS (IZQ Y DER) ---
                        mainCol.Item().Row(row =>
                        {
                            // ==========================================
                            // COLUMNA IZQUIERDA: IMAGEN Y DATOS DEL AUTO
                            // ==========================================
                            row.RelativeItem(1).PaddingRight(15).Column(leftCol =>
                            {
                                // Imagen del auto
                                leftCol.Item().AlignCenter().Width(180).Image("Assets/magnite.png"); // Reemplaza con tu imagen

                                // Nombre del auto grande y centrado

                                // Bloque: Datos del Auto
                                leftCol.Item().PaddingTop(5).Element(compose => ComposeBlockTable(compose, "Datos del Auto", datosAuto, color));

                                // Bloque: Seguro
                                leftCol.Item().PaddingTop(5).Element(compose => ComposeBlockTable(compose, "Seguro", datosSeguro, color));

                                // Bloque: Garantía Extendida
                                leftCol.Item().PaddingTop(5).Element(compose => ComposeBlockTable(compose, "Garantía Extendida", datosGarantia, color));
                            });

                            // ==========================================
                            // COLUMNA DERECHA: PLANES Y TOTALES
                            // ==========================================
                            row.RelativeItem(1).PaddingLeft(15).Column(rightCol =>
                            {
                                // Totales grandes resaltados
                                rightCol.Item().AlignCenter().PaddingTop(20).Text(x =>
                                {
                                    x.Span("Mensualidad: ").FontSize(16).SemiBold().FontColor(Colors.Red.Medium);
                                    x.Span("$9,402.88").FontSize(16).SemiBold();
                                });

                                rightCol.Item().PaddingTop(5).AlignCenter().Text(x =>
                                {
                                    x.Span("Plazo: ").FontSize(14).SemiBold().FontColor(Colors.Red.Medium);
                                    x.Span("72 meses").FontSize(14).SemiBold();
                                });

                                // Bloque: Datos del Plan (El más grande)
                                rightCol.Item().PaddingTop(25).Element(compose => ComposeBlockTable(compose, "Datos del Plan", datosPlan, color));

                                // Bloque: Productos Adicionales
                                rightCol.Item().PaddingTop(5).Element(compose => ComposeBlockTable(compose, "Productos Adicionales", datosAdicionales, color));
                            });
                        });

                        mainCol.Item().Extend().PaddingTop(5).Element(container =>
                        {
                            // Este contenedor se alinea al fondo del espacio restante
                            container.AlignBottom().Image("Assets/back_publicidad.png");
                        });

                        mainCol.Item().PageBreak();

                        mainCol.Item().AlignLeft().Text("TABLA DE AMORTIZACION").FontSize(18);

                        mainCol.Item().PaddingTop(10).Table(table =>
                        {
                            // Definir columnas de la tabla de productos
                            table.ColumnsDefinition(columns =>
                            {
                                columns.ConstantColumn(33); // ID o #
                                columns.ConstantColumn(43);  // Descripción/Producto
                                columns.RelativeColumn(1);  // Cantidad
                                columns.RelativeColumn(1);  // Precio
                                columns.RelativeColumn(1);  // Precio
                                columns.RelativeColumn(1);  // Precio
                                columns.RelativeColumn(1);  // Precio
                                columns.RelativeColumn(1);  // Precio
                                columns.RelativeColumn(1);  // Precio
                                columns.RelativeColumn(1);  // Precio
                                columns.RelativeColumn(1);  // Precio
                                columns.RelativeColumn(1);  // Precio
                                columns.RelativeColumn(1);  // Precio
                            });

                            // Encabezado de la tabla (Se repetirá en cada página si la tabla es larga)
                            table.Header(header =>
                            {
                                header.Cell().Element(HeaderStyle).Text("Periodo");
                                header.Cell().Element(HeaderStyle).Text("fecha de pago");
                                header.Cell().Element(HeaderStyle).AlignCenter().Text("Saldo del auto");
                                header.Cell().Element(HeaderStyle).AlignCenter().Text("capital");
                                header.Cell().Element(HeaderStyle).AlignCenter().Text("interes");
                                header.Cell().Element(HeaderStyle).AlignCenter().Text("iva de interes");
                                header.Cell().Element(HeaderStyle).AlignCenter().Text("seguro de auto");
                                header.Cell().Element(HeaderStyle).AlignCenter().Text("seguro de vida");
                                header.Cell().Element(HeaderStyle).AlignCenter().Text("accesorios");
                                header.Cell().Element(HeaderStyle).AlignCenter().Text("garantia extendida");
                                header.Cell().Element(HeaderStyle).AlignCenter().Text("Productos adicional");
                                header.Cell().Element(HeaderStyle).AlignCenter().Text("Pago CXA");
                                header.Cell().Element(HeaderStyle).AlignCenter().Text("Pago mensual");

                                // Estilo local para el encabezado de la tabla
                                IContainer HeaderStyle(IContainer container) =>
                                    container
                                        .Background(color)
                                        .BorderBottom(1)
                                        .BorderColor(Colors.White)
                                        .Padding(2)
                                        .DefaultTextStyle(x => x.FontColor(Colors.White).FontSize(6));
                            });

                            // Iteración de tus productos (Información Dinámica)
                            foreach (var producto in productos)
                            {
                                table.Cell().Element(RowStyle).AlignCenter().Text(producto.Id.ToString());
                                table.Cell().Element(RowStyle).Text(producto.fecha_pago);
                                table.Cell().Element(RowStyle).AlignCenter().Text($"${producto.Precio:N2}");
                                table.Cell().Element(RowStyle).AlignCenter().Text($"${producto.Precio:N2}");
                                table.Cell().Element(RowStyle).AlignCenter().Text($"${producto.Precio:N2}");
                                table.Cell().Element(RowStyle).AlignCenter().Text($"${producto.Precio:N2}");
                                table.Cell().Element(RowStyle).AlignCenter().Text($"${producto.Precio:N2}");
                                table.Cell().Element(RowStyle).AlignCenter().Text($"${producto.Precio:N2}");
                                table.Cell().Element(RowStyle).AlignCenter().Text($"${producto.Precio:N2}");
                                table.Cell().Element(RowStyle).AlignCenter().Text($"${producto.Precio:N2}");
                                table.Cell().Element(RowStyle).AlignCenter().Text($"${producto.Precio:N2}");
                                table.Cell().Element(RowStyle).AlignCenter().Text($"${producto.Precio:N2}");
                                table.Cell().Element(RowStyle).AlignCenter().Text($"${producto.Precio:N2}");

                                // Estilo para las filas
                                static IContainer RowStyle(IContainer container) =>
                                    container.BorderBottom(1).BorderColor(Colors.Grey.Lighten2).PaddingHorizontal(3).PaddingVertical(6)
                                    .DefaultTextStyle(x => x.FontColor(Colors.Black).FontSize(6)); ;
                            }
                        });
                    });

                    // --- PIE DE PÁGINA (Se repite en cada página) ---
                    page.Footer().Element(container => ComposeFooter(container, color));
                });
            });
            // Muestra en la aplicación Companion (reemplaza al antiguo Previewer)
            documento.ShowInCompanion();
            // Retornar como array de bytes para la Web API
            return documento.GeneratePdf();
        }

        private void ComposeHeader(IContainer container, string colorPrimario)
        {
            container.Column(HeaderCol =>
            {
                HeaderCol.Item().Row(row =>
                {
                    row.RelativeItem().Column(col =>
                    {
                        col.Item().Text(DateTime.Now.ToString()).FontSize(6).SemiBold().FontColor(Colors.Grey.Medium);
                    });

                    row.ConstantItem(150).Image("Assets/logo_credi2.png");

                });

                // Línea roja divisoria
                HeaderCol.Item().PaddingTop(5).LineHorizontal(5).LineColor(colorPrimario);
            });
        }

        private void ComposeBlockTable(IContainer container, string titulo, Dictionary<string, string> datos, string colorHex = "#D32F2F")
        {
            container.Table(table =>
            {
                // Definición de columnas: Clave (proporción 1) y Valor (proporción 2)
                table.ColumnsDefinition(columns =>
                {
                    columns.RelativeColumn(2);
                    columns.RelativeColumn(3);
                });

                // ENCABEZADO ROJO DEL BLOQUE
                table.Header(header =>
                {
                    header.Cell().ColumnSpan(2) // Ocupa ambas columnas
                        .Background(colorHex)
                        .PaddingVertical(2)
                        .PaddingHorizontal(5)
                        .AlignCenter()
                        .Text(titulo).FontSize(11).SemiBold().FontColor(Colors.White);
                });

                // FILAS DE DATOS DENTRO DEL BLOQUE
                foreach (var item in datos)
                {
                    // Estilo para la Clave
                    table.Cell().PaddingLeft(5).PaddingRight(0).PaddingVertical(1)
                        .Text(item.Key).FontSize(10).SemiBold();

                    // Estilo para el Valor
                    table.Cell().PaddingLeft(5).PaddingRight(1).PaddingVertical(1)
                        .Text(item.Value).FontSize(10).FontColor(Colors.Grey.Darken2);
                }
            });
        }

        private void ComposeFooter(IContainer container, string colorPrimario, string catValor = "18.3%", string fechaCalculo = "09/03/26")
        {
            container.Column(footerCol =>
            {
                // 1. Línea decorativa superior
                footerCol.Item().PaddingBottom(5).LineHorizontal(3).LineColor(colorPrimario);

                // 2. Texto Legal (Tamaño pequeño y Justificado)
                footerCol.Item().PaddingTop(2).Text("El cliente deberá contratar una póliza de seguro con " +
                    "cobertura amplia con una vigencia equivalente al plazo del crédito. " +
                    "Los precios contenidos yo desplegados en esta página son válidos en toda la República Mexicana y se encuentran proyectados " +
                    "en pesos mexicanos.Los precios, especificaciones, versiones, cantidades y resultados contenidos yo desplegados en esta página," +
                    " se han elaborado con fines exclusivamente informativos, por lo que en ningún caso representan oferta yo compromiso alguno para NR Finance México S.A. de C.V.," +
                    " yo los distribuidores, por lo que éstos no asumen responsabilidad alguna por la precisión y uso que el usuario de esta página le dé a tal información." +
                    " NRFM se reserva la facultad de modificar, en cualquier momento, la información contenida en esta calculadora financiera, sin previo aviso. " +
                    "Los montos de las mensualidades indicadas no incluyen comisiones,seguros, impuestos yo demás cargas financieras y " +
                    "han sido calculados tomando como base una tasa de interés que puede variar sin previo aviso. Por lo anterior, NRFM le recomienda consultar los términos y condiciones de financiamiento, " +
                    "así como las condiciones de adquisición, disponibilidad de modelos, versiones yo colores de los vehículos, " +
                    "al momento de realizar la operación de financiamiento yo adquisición con el distribuidor de selección. Sujeto a aprobación de crédito. " +
                    "Los planes de seguro gratis no incluyen no incluyen el seguro de los accesorios. De conformidad con lo previsto en el artículo Ley General de Organizaciones y " +
                    "Actividades Auxiliares del Crédito, NRFM no requiere de autorización de la Secretaría de Hacienda " +
                    "y Crédito Público para organizarse.").FontColor(Colors.Black).FontSize(6).Justify();

                // 3. Bloque del CAT
                footerCol.Item().PaddingTop(5).Text(text =>
                {
                    text.DefaultTextStyle(s => s.FontSize(8).FontColor(Colors.Black));
                    text.AlignLeft();

                    text.Span($"CAT PROMEDIO de {catValor} ").Bold();
                    text.Span($"Sin IVA, calculado el {fechaCalculo} Para fines informativos y de comparación exclusivamente. ");
                    text.Span("Monto total del seguro $71,771.09 M.N. por un plazo de 72 meses.");
                });

                // 4. Paginación
                footerCol.Item().AlignRight().Text(x =>
                {
                    x.DefaultTextStyle(s => s.FontSize(8));
                    x.Span("Página ");
                    x.CurrentPageNumber();
                    x.Span(" de ");
                    x.TotalPages();
                });
            });
        }
    }

    public class Producto
    {
        public string Id { get; set; }
        public string Nombre { get; set; }
        public string fecha_pago { get; set; }
        public double Precio { get; set; }
        public int Cantidad { get; set; }
    }

    public class CotizacionVehiculo
    {
        public string ImagenRuta { get; set; } // O byte[]
        public string NombreModelo { get; set; }
        public decimal Mensualidad { get; set; }
        public int PlazoMeses { get; set; }

        // Sub-datos clave-valor
        public Dictionary<string, string> DatosAuto { get; set; }
        public Dictionary<string, string> DatosPlan { get; set; }
        public Dictionary<string, string> DatosSeguro { get; set; }
        // ... otros
    }
}

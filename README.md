# Trabajo Práctico Integrador
## Desarrollo de Software
### Backend
INTEGRANTES

57481 - Mercado Agostina - Agostina.Mercado@alu.frt.utn.edu.ar

56838 - Lagoria Villagran Luciano Emanuel - Luciano.LagoriaVillagran@alu.frt.utn.edu.ar

21204 - Karina Del Valle Miranda - Karina.Miranda@frt.utn.edu.ar

## INSTRUCCIONES PARA USO PERSONAL
-Abrir la solución en Visual Studio
-En la barra superior, hacé clic en Herramientas.
-Luego seleccioná: Administrador de paquetes NuGet → Consola del Administrador de paquetes.
-Se abrirá una ventana en la parte inferior del IDE donde migramos la base de datos
-Ejecutamos el comando: Update-Database
-La bdd ya esta funcionando, ya con las ultimas migraciones del modelo

## 🚀 Endpoints – Productos y Órdenes (1 al 6)

### 1. Crear un producto
- **Método:** POST  
- **Ruta:** `/api/products`  
- **Descripción:** Crea un nuevo producto con campos obligatorios como SKU, nombre, descripción, precio y stock.
- **Request Body ejemplo:**
```json
{
  "sku": "ABC123",
  "internalCode": "INT-001",
  "name": "Teclado Gamer",
  "description": "Teclado con retroiluminación y switches azules",
  "currentUnitPrice": 24999.99,
  "stockQuantity": 30
}
```
Respuestas esperadas:

-201 Created: Producto creado correctamente.

-400 Bad Request: Datos inválidos o SKU duplicado.

### 2. Obtener todos los productos
- **Método:** GET  
- **Ruta:** `/api/products`  
- **Descripción:** Devuelve todos los productos activos (IsActive = true).
Respuestas esperadas:

-200 OK: Lista de productos.

-204 No Content: No hay productos activos.

### 3. Obtener un producto por ID
- **Método:** GET  
- **Ruta:** `/api/products/{id}`  
- **Descripción:** evuelve el producto que coincida con el ID provisto.
Respuestas esperadas:

-200 OK: Producto encontrado.

-404 Not Found: Producto inexistente.

### 4. Actualizar un producto
- **Método:** PUT  
- **Ruta:** `/api/products/{id}`  
- **Descripción:** Actualiza todos los campos de un producto dado su ID.
- **Request Body ejemplo:**
```json
{
  "sku": "ABC123",
  "internalCode": "INT-001",
  "name": "Teclado Gamer RGB",
  "description": "Nueva descripción actualizada",
  "currentUnitPrice": 25999.99,
  "stockQuantity": 40
}
```
Respuestas esperadas:

-200 OK: Producto actualizado.

-400 Bad Request: Datos inválidos.

-404 Not Found: Producto inexistente.

### 5. Inhabilitar un producto
- **Método:** PATCH  
- **Ruta:** `/api/products/{id}`  
- **Descripción:** Marca el producto como inactivo (IsActive = false) sin eliminarlo.
Respuestas esperadas:
-204 No Content: Producto inhabilitado.
-404 Not Found: Producto no encontrado.

### 6. Crear una Orden
- **Método:** POST  
- **Ruta:** `/api/orders`  
- **Descripción:** Registra una nueva orden para un cliente. Valida que los productos existan, estén activos, coincidan en precio y tenga stock suficiente.
- **Request Body ejemplo:**
```json
{
  "customerId": "guid-del-cliente",
  "shippingAddress": "Av. Mitre 123",
  "billingAddress": "Av. Rivadavia 456",
  "orderItems": [
    {
      "productId": "guid-del-producto",
      "quantity": 2,
      "currentUnitPrice": 15999.50,
      "name": "Auriculares Bluetooth",
      "description": "Auriculares inalámbricos con cancelación de ruido y hasta 20 horas de batería."
    }
  ]
}
```
Respuestas esperadas:

-201 Created: Orden creada exitosamente.

-400 Bad Request: Cliente inválido, stock insuficiente o productos inconsistentes.

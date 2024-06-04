# Antes de Empezar 
Requisitos para levantar el proyecto:

- Node.js v20.13.1+
- npm 10.8.0+
- .NET 8.0.6
- .NET SDK 8.0.301

## Cómo empezar
Esta plantilla está diseñada para ayudarte a configurar rápidamente un proyecto Blazor con Flowbite. Si deseas conocer más sobre el proceso y entender los pasos en mayor profundidad, puedes consultar este enlace: https://flowbite.com/docs/getting-started/blazor/

Para obtener la plantilla, solo sigue estas simples instrucciones:

1. Clona el repositorio en una carpeta local en tu máquina.
```bash
git clone https://github.com/LuisLopez-developer/EccomerceBlazorWasm
```

2. Abre la solución del proyecto.

3. En una ventana de terminal, instala las dependencias necesarias con:
```bash
npm install
```
4. Listo, para finalizar ejecuta:
```bash
dotnet watch
```

> [!important]
> Para que los cambios **CSS** se **copilen al momento**, en una terminal ejecuta:
> ```bash
> npx tailwindcss -i wwwroot/css/app.css -o wwwroot/css/app.min.css --watch
> ```
> y en otra:
> ```bash
> dotnet watch
> ```

## Creditos especiales
A: [**Rasheed K Mozaffar**](https://github.com/rasheed-k-mozaffar/FlowbiteBlazorWasmStarter) 

Por: La base del proyecto, con las modificaciones de Blazor para utilizar [**Tailwind CSS**](https://tailwindcss.com/) junto con [**Flowbite**](https://flowbite.com/).

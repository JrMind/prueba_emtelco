# DragonBall Battle Scheduler API

API REST en .NET 6 con arquitectura limpia para organizar batallas aleatorias utilizando la API publica de DragonBall.

## Proyectos
- **Domain**: Entidades del dominio.
- **Application**: Interfaces, DTOs y servicios de negocio.
- **Infrastructure**: Implementaciones externas (API DragonBall).
- **WebAPI**: API ASP.NET Core con JWT, Swagger y logging.
- **Tests**: Pruebas unitarias con xUnit.

## Uso
1. Solicitar emparejamientos enviando un número par de luchadores entre 2 y 16 al endpoint `/api/battles`. Se requiere autenticación JWT.
2. El servicio consulta la API de DragonBall, genera batallas aleatorias (dos por día) comenzando 30 días después y retorna el cronograma.

Ejemplo de respuesta:
```json
[
  {
    "date": "2023-12-01T00:00:00Z",
    "fighter1": "Goku",
    "fighter2": "Vegeta"
  }
]
```

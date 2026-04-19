# PlayerHost Game - TCP Unity

## Descripción general del proyecto
Este proyecto consiste en un juego multijugador desarrollado en Unity utilizando comunicación por TCP, este sistema permite la conexión entre un host (servidor) y múltiples clientes, donde los jugadores interactúan en tiempo real dentro de un entorno 3D.

El objetivo del juego es que el jugador host (rojo) persiga y capture a los jugadores clientes (azules), mientras ellos no pueden ver quien es el host, cuando el host logra atrapar a un jugador, el juego finaliza y se cambia a una escena de Game Over.

---

## Instrucciones para ejecutar el sistema

### Ejecutar como Host
1. Abrir el juego.
2. Presionar el botón **Host**.
3. Se iniciará el servidor automáticamente.
4. El jugador host aparecerá en la escena de Game.

---

### Ejecutar como Cliente
1. Abrir el juego en otra instancia o en otro equipo.
2. Ingresar la dirección IP del host en el campo de texto.  
   Ejemplo: `127.0.0.1` (en la misma computadora)
3. Presionar el botón **Conectar**.
4. El cliente se unirá a la partida.

---

## Requisitos técnicos
- Unity (versión recomendada: Unity 6)
- Conexión de red local (LAN) o localhost
- Sistema operativo Windows (para ejecución en .exe)

---

## Funcionalidades implementadas

- Conexión cliente-servidor mediante TCP  
- Comunicación multidireccional entre host y clientes  
- Movimiento sincronizado de jugadores  
- Detección de colisiones (para captura del jugador)  
- Cambio de escena al finalizar el juego  
- Sistema de pausa global controlado por el host  
- Expulsión de jugadores (kick)  
- Visualización de latencia (ping)  
- Interfaz de usuario con TextMeshPro  
- Validación de IP antes de la conexión  
- Soporte para múltiples clientes  

---

## Errores o limitaciones conocidos

- No existe reconexión automática en caso de pérdida de conexión.
- No hay gestión de múltiples partidas simultáneas (todos los jugadores comparten la misma sesión).
- Puede haber diferencias de comportamiento o visuales entre el Editor de Unity y el build (.exe).

---


## Nota

Este proyecto fue desarrollado con fines académicos para comprender los fundamentos de redes en videojuegos, incluyendo:

- Comunicación cliente-servidor  
- Sincronización  
- Manejo de estados del juego  
- Interacción en tiempo real  

---

## Autores

Luis Miguel Guerrero y Juan David Goyeneche

# PlayerHost Game - TCP Unity

## Descripción general del proyecto
Este proyecto consiste en un juego multijugador desarrollado en Unity utilizando comunicación por TCP, este sistema permite la conexión entre un host (servidor) y múltiples clientes, donde los jugadores interactúan en tiempo real dentro de un entorno 3D.

El objetivo del juego es que el jugador host (rojo) persiga y capture a los jugadores clientes (azules). 
Los jugadores clientes no pueden identificar quién es el host, lo que añade un componente de incertidumbre, cuando el host logra atrapar a un jugador, el juego finaliza y se cambia a una escena de Game Over.

---

## Objetivo del juego
El objetivo principal es generar una experiencia multijugador simple en la que:

- El host tiene el rol de perseguidor.
- Los clientes deben evitar ser capturados.
- Se pone en práctica la sincronización de jugadores en red.
- Se introduce una mecánica de juego basada en interacción en tiempo real.

---

## Flujo del juego

### Inicio
- El usuario inicia la aplicación.
- Puede elegir entre ser Host o Cliente.
- El host crea la partida y los clientes se conectan mediante una dirección IP.
- Una vez conectados, todos los jugadores aparecen en la escena principal.

### Desarrollo
- Los jugadores se mueven libremente en el escenario.
- El host intenta identificar y perseguir a los demás jugadores.
- Los clientes intentan evadir al host sin saber quién es.
- El sistema sincroniza posiciones en tiempo real mediante mensajes TCP.
- El host puede pausar el juego o expulsar jugadores.

### Final
- Cuando el host colisiona con un jugador cliente:
  - Se envía el mensaje de finalización.
  - Todos los jugadores cambian a la escena de Game Over.
- El juego termina y puede reiniciarse o salir del juego desde el menú.

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

## Conclusiones

- Se logró implementar correctamente un sistema multijugador básico utilizando TCP, permitiendo la comunicación en tiempo real entre múltiples jugadores.
- El proyecto permitió comprender conceptos fundamentales de redes como cliente-servidor, envío de mensajes y sincronización de estados.
- Se evidenció la importancia de manejar correctamente la conexión y desconexión de clientes.
- A pesar de las limitaciones y errores presentes durante el desarrollo, se logró completar el proyecto de manera exitosa, cumpliendo con los objetivos planteados inicialmente por el profesor.

---

## Integrantes

Luis Miguel Guerrero  
Juan David Goyeneche  

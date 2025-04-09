using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class LobbyTeams : MonoBehaviourPunCallbacks
{
    // Variables para las zonas de los equipos
    public Transform equipo1Zona;
    public Transform equipo2Zona;

    private void Start()
    {
        // Asignar equipos y posicionar jugadores cuando comienza el juego
        AsignarEquipos();
        PosicionarJugadores();
    }

    // Se llamará al entrar o salir un jugador de la sala
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        AsignarEquipos();
        PosicionarJugadores();
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        AsignarEquipos();
        PosicionarJugadores();
    }

    // Asignar jugadores a los equipos
    void AsignarEquipos()
    {
        int numJugadores = PhotonNetwork.PlayerList.Length;

        // Recorremos todos los jugadores y los asignamos a un equipo
        for (int i = 0; i < numJugadores; i++)
        {
            Player jugador = PhotonNetwork.PlayerList[i];
            int equipo = (i % 2 == 0) ? 1 : 2; // Alternamos entre los equipos 1 y 2

            // Si el número de jugadores es impar, balanceamos los equipos
            if (numJugadores % 2 != 0)
            {
                equipo = (i < numJugadores / 2) ? 1 : 2;
            }

            // Asignamos el equipo al jugador
            jugador.SetCustomProperties(new ExitGames.Client.Photon.Hashtable() { { "Equipo", equipo } });
        }
    }

    // Posicionar los jugadores en el campo
    void PosicionarJugadores()
    {
        int numJugadores = PhotonNetwork.PlayerList.Length;

        for (int i = 0; i < numJugadores; i++)
        {
            Player jugador = PhotonNetwork.PlayerList[i];
            int equipo = (int)jugador.CustomProperties["Equipo"]; // Obtenemos el equipo del jugador

            // Usamos las posiciones definidas en el Inspector (equipos 1 y 2)
            Vector3 posicion = equipo == 1 ? equipo1Zona.position : equipo2Zona.position;

            // Aseguramos que la posición en Z varíe para evitar que los jugadores se solapen
            posicion.z += i * 2f;  // Esto aumenta la posición en Z para que no se solapen

            // Asignamos la posición al jugador
            PhotonView photonView = jugador.TagObject as PhotonView;
            if (photonView != null)
            {
                photonView.transform.position = posicion;
            }
        }
    }

    // Para ver las zonas en el Editor de Unity como Gizmos
    private void OnDrawGizmos()
    {
        if (equipo1Zona != null && equipo2Zona != null)
        {
            // Gizmo para la zona del Equipo 1
            Gizmos.color = Color.red; // Rojo para el equipo 1
            Gizmos.DrawWireCube(equipo1Zona.position, new Vector3(5f, 5f, 5f)); // Ajusta el tamaño de la zona

            // Gizmo para la zona del Equipo 2
            Gizmos.color = Color.yellow; // Amarillo para el equipo 2
            Gizmos.DrawWireCube(equipo2Zona.position, new Vector3(5f, 5f, 5f)); // Ajusta el tamaño de la zona
        }
    }
}

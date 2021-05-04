
import { HubConnectionBuilder} from "@microsoft/signalr";
export const getters = {
  isAuthenticated(state) {
    return state.auth.loggedIn
  },

  loggedInUser(state) {
    return state.auth.user
  },
    connection(connection) {
    if (connection === null) {
      connection = new HubConnectionBuilder()
        .withUrl("https://localhost:44387/chatHub")
        .build();
    }
    return HubConnectionBuilder.connection
  }
}


import { HubConnectionBuilder} from "@microsoft/signalr";
export const getters = {
  isAuthenticated(state) {
    return state.auth.loggedIn
  },

  loggedInUser(state) {
    return state.auth.user
  },
    
}
export const state = () => ({
  connection: new HubConnectionBuilder()
})
export const mutations = {
  set(state, connection) {
    state.connection = connection
  }
}

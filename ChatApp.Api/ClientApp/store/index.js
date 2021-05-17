export const state = () => ({
  messages: []
})

export const getters = {
  isAuthenticated(state) {
    return state.auth.loggedIn
  },

  loggedInUser(state) {
    return state.auth.user
  },
    
}
export const mutations = {

  add (state,message, user,isRecived) {
    state.messages.push({
      Message: message,
      User: user,
      IsRecived: isRecived,
    })
  }
}

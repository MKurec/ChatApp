<template>
  <v-row justify="center">
    <v-dialog v-model="dialog" persistent max-width="600px">

      <v-card>
        <v-card-title>
          <span class="headline">Zaloguj</span>
        </v-card-title>
        <v-card-text>
          <v-container>
            <v-row>
              <v-col cols="12">
                <v-text-field label="Email*" v-model="login.email" required></v-text-field>
              </v-col>
              <v-col cols="12">
                <v-text-field
                  v-model="login.password"
                  :append-icon="show1 ? 'mdi-eye' : 'mdi-eye-off'"
                  :type="show1 ? 'text' : 'password'"
                  name="input-10-1"
                  label="Hasło"
                  hint="At least 8 characters"
                  counter
                  @click:append="show1 = !show1"
                ></v-text-field>
              </v-col>
             </v-row>
          </v-container>
          <small>*indicates required field</small>
        </v-card-text >
        <v-card-actions class="d-flex justify-space-between">
          <NuxtLink to="/register" no-prefetch><v-btn color="blue darken-1" text >
            Rejestracja
          </v-btn></NuxtLink>
          <v-btn color="blue darken-1" text @click="userLogin">
            Zaloguj
          </v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>
  </v-row>
</template>

<script>
export default {
  data() {
    return {
      login: {
        email: '',
        password: ''
      },
      show1: false,
      dialog: true,
    }
  },
  methods: {
    async userLogin() {
      try {
        let response = await this.$auth.loginWith('local', { data: this.login })
        console.log(response)
      } catch (err) {
        console.log(err)
      }
    }
  },

  middeware: 'guest'

}
</script>
<template>
  <div class="d-flex algin-center justify-center">
    <Alert v-model="dialog" :errmesage="errorMessage" />
    <v-dialog v-model="dialog" persistent max-width="600px">
      <v-card>
        <v-card-title>
          <span class="headline">Załóż konto</span>
        </v-card-title>
        <v-card-text>
          <v-container>
            <v-row>
              <v-col cols="12">
                <v-text-field
                  v-model="register.firstName"
                  label="Imię"
                  required
                ></v-text-field>
              </v-col>
              <v-col cols="12">
                <v-text-field
                  v-model="register.email"
                  label="Email*"
                  required
                ></v-text-field>
              </v-col>
              <v-col cols="12">
                <v-text-field
                  v-model="register.password"
                  :append-icon="show1 ? 'mdi-eye' : 'mdi-eye-off'"
                  :type="show1 ? 'text' : 'password'"
                  name="input-10-1"
                  label="Hasło"
                  hint="At least 8 characters"
                  counter
                  @click:append="show1 = !show1"
                ></v-text-field>
              </v-col>
              <v-col cols="12">
                <v-text-field
                  v-model="checkPassword"
                  label="Hasło*"
                  type="password"
                  required
                ></v-text-field>
                <v-file-input
                  v-model="photo"
                  label="Dodaj Zdjęcie"
                  accept="image/png, image/jpeg, image/bmp"
                  prepend-icon="mdi-camera"
                ></v-file-input>
              </v-col>
            </v-row>
          </v-container>
          <small>*indicates required field</small>
        </v-card-text>
        <v-card-actions class="d-flex justify-space-between">
          <NuxtLink to="/" no-prefetch
            ><v-btn color="blue darken-1" text> Zamknij </v-btn></NuxtLink
          >
          <v-btn color="blue darken-1" text @click="registerUser">
            Dalej
          </v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>
  </div>
</template>
<script>
import alert from "../components/Alert.vue";
export default {
  components: { alert },
  data() {
    return {
      register: {
        name: "",
        email: "",
        password: "",
      },
      login: {
        email: "",
        password: "",
      },
      show1: false,
      dialog: true,
      checkPassword: "",
      errorMessage: "",
      photo: null,
      userId: "",
    };
  },
  methods: {
    async registerUser() {
      await this.$axios
        .post("/api/Users/register", {
          name: this.register.firstName,
          email: this.register.email,
          password: this.register.password,
        })
        .catch((err) => {
          (this.errorMessage = err.response.data.message),
            (this.dialog = false);
        });
      if (this.dialog == true) {
        this.login.email=this.register.email
        this.login.password=this.register.password
        try {
          let response = await this.$auth.loginWith("local", {
            data: this.login,
          });
          console.log(response),
          this.addPhoto();
        } catch (err) {
          console.log(err);
        }
      }
    },
    async addPhoto() {
      const fd = new FormData();
      if (this.photo) {
        fd.append("photo", this.photo, this.photo.name);
        await this.$axios.post("https://localhost:44387/Users/AddPhoto", fd, {
          headers: { "Content-Type": "multipart/form-data" },
        });
      }
    },
  },
};
</script>
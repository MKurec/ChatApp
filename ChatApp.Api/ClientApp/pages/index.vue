
<template>
  <div>
    <v-toolbar color="deep-purple accent-4" dark>
      <v-app-bar-nav-icon></v-app-bar-nav-icon>

      <v-toolbar-title> {{ loggedInUser.name }}</v-toolbar-title>

      <v-spacer></v-spacer>

      <v-btn @click="logout" icon>
        <v-icon>mdi-magnify</v-icon>
      </v-btn>
    </v-toolbar>

    <v-row>
      <v-col style="max-width: 400px" flex-grow="0">
        <v-list subheader>
          <v-subheader>Users </v-subheader>
          <v-list-item-group v-model="selectedUser" color="primary">
            <v-list-item v-for="user in users" :key="user.id">
              <v-list-item-avatar>
                <v-img
                  :alt="`${user.name} avatar`"
                  :src="'https://localhost:44387/Users/Image/'+user.id"
                ></v-img>
              </v-list-item-avatar>

              <v-list-item-content>
                <v-list-item-title v-text="user.name"></v-list-item-title>
              </v-list-item-content>
              <v-list-item-icon>
                <v-icon :color="user.isChatActive ? 'deep-purple accent-4' : 'grey'">
                  mdi-message-outline
                </v-icon>
              </v-list-item-icon>
            </v-list-item>
          </v-list-item-group>
        </v-list>
      </v-col>
      <v-col v-if="selectedUser">
        <v-container>
          <div v-for="thismessage in messages" :key="thismessage">
            <v-row
              class="d-flex justify-start pr-6 pa-3"
              v-if="
                thismessage.user == users[selectedUser].id &&
                thismessage.isRecived == true
              "
            >
              <v-avatar size="25px">
                <img
                  alt="Avatar"
                  :src="'https://localhost:44387/Users/Image/'+users[selectedUser].id"
                />
              </v-avatar>
              <!--  confusing naming below -->

              <v-textarea
                :value="thismessage.message"
                rows="1"
                dense
                auto-grow
                outlined
                readonly
                rounded
              ></v-textarea>
            </v-row>
            <v-row
              class="d-flex justify-end pr-6 pa-3"
              v-if="
                thismessage.user == users[selectedUser].id &&
                thismessage.isRecived == false
              "
            >
              <!--  confusing naming below -->

              <v-textarea
                :value="thismessage.message"
                dense
                rows="1"
                auto-grow
                outlined
                readonly
                rounded
              ></v-textarea>
              <v-avatar size="25px">
                <img
                  alt="Avatar"
                  :src="'https://localhost:44387/Users/Image/'+users[selectedUser].id"
                />
              </v-avatar>
            </v-row>
          </div>
        </v-container>
        <v-form>
          <v-container>
            <v-row>
              <v-col cols="12">
                <v-text-field
                  v-model="messageContent"
                  :append-icon="
                    marker ? 'mdi-map-marker' : 'mdi-map-marker-off'
                  "
                  :append-outer-icon="messages ? 'mdi-send' : 'mdi-microphone'"
                  filled
                  clear-icon="mdi-close-circle"
                  clearable
                  label="Message"
                  type="text"
                  @click:append="toggleMarker"
                  @click:append-outer="sendMessage"
                ></v-text-field>
              </v-col>
            </v-row>
          </v-container>
        </v-form>
      </v-col>
    </v-row>
  </div>
</template>


<script>
import { mapGetters } from "vuex";
import { HubConnectionBuilder, HubConnectionState } from "@microsoft/signalr";

const signalR = require("@microsoft/signalr");

export default {
  props: {},
  data: () => {
    return {
      password: "Password",
      show: false,
      messageContent: "",
      marker: true,
      dialog: false,
      name: "",
      connection: null,
      users: [],
      messages: [],
      userMessage: "",
      selectedUser: null,
    };
  },
  computed: {
    ...mapGetters(["loggedInUser"]),
    //...mapState({      connection: state => state.connection
    // })
  },
  watch: {
    selectedUser: function(){
      if (this.users[this.selectedUser].isChatActive == true)
      {
        this.users[this.selectedUser].isChatActive = false,
        this.$axios.put("https://localhost:44387/Users/UnActiveUser/"+this.users[this.selectedUser].id)
        console.log(this.users[this.selectedUser].id)
      }
    }
  },
  methods: {
    async logout() {
      await this.$auth.logout(), this.$router.replace("/login");
    },
    toggleMarker() {
      (this.marker = !this.marker), this.created(), this.mounted();
    },
    sendMessage() {
      if (this.selectedUser)
        this.connection
          .invoke(
            "SendMessage",
            this.messageContent,
            this.users[this.selectedUser].id
          )
          .catch(function (err) {
            return console.error(err);
          })
          .then(() => {
            var messageObj = {
              message: this.messageContent,
              user: this.users[this.selectedUser].id,
              isRecived: false,
            };
            this.messageContent = "";
            this.messages.push(messageObj);
          });
    },
    openDialog() {
      this.dialog = true;
    },
    listen() {
      console.log("Listen Started");
      this.connection.on("OnConnectedAsync", (res) => {
        console.log(res);
      });
      this.connection.on("ReciveMessage", (res, res2) => {
        var messageObj = {
          message: res,
          user: res2,
          isRecived: true,
        };
        this.messages.push(messageObj);
        this.users[messageObj.user].isChatActive=true;
        console.log(res);
      });
    },
    getUsers: async function () {
      let users = await this.$axios.$get("/api/Users/Users/");
      this.users = users;
    },
    getMessages: async function () {
      let RecivedMessages = await this.$axios.$get(
        "https://localhost:44387/Messages/"
      );
      RecivedMessages.forEach((message) => {
        var messageObj = {
          message: message.text,
          user: message.receiverId,
          isRecived: message.isRecived,
        };
        this.messages.push(messageObj);
      });
    },
  },

  middleware: "auth",
  created() {
    if (this.connection == null) {
      this.connection = new HubConnectionBuilder()
        .withUrl("https://localhost:44387/chatHub", {
          accessTokenFactory: () =>
            this.$auth.strategy.token.get().substring(7),
        })
        .configureLogging(signalR.LogLevel.Information)
        .build();
    }
    console.log(this.connection.state, "sd");
    this.connection
      .start()
      .then(() => {
        console.log(this.connection.state),
          console.log("SignalR Connected."),
          this.listen(),
          this.getUsers(),
          this.getMessages();
      })
      .catch((err) => {
        console.log(`Connection Error ${err}`);
      });
    this.connection.onclose(() => {
      console.log("Connection Destroy");
    });
  },
};
</script>

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
          <v-subheader>Recent chat</v-subheader>

          <v-list-item v-for="chat in recent" :key="chat.avatar">
            <v-list-item-avatar>
              <v-img :alt="`${chat.title} avatar`" :src="chat.avatar"></v-img>
            </v-list-item-avatar>

            <v-list-item-content>
              <v-list-item-title v-text="chat.title"></v-list-item-title>
            </v-list-item-content>

            <v-list-item-icon>
              <v-icon :color="activechat ? 'deep-purple accent-4' : 'grey'">
                mdi-message-outline
              </v-icon>
            </v-list-item-icon>
          </v-list-item>
        </v-list>

        <v-divider></v-divider>

        <v-list subheader>
          <v-subheader>Previous chats </v-subheader>
          <v-list-item-group v-model="selectedUser" color="primary">
            <v-list-item v-for="user in users" :key="user.id">
              <v-list-item-avatar>
                <v-img
                  :alt="`${user.name} avatar`"
                  src="https://cdn.vuetifyjs.com/images/lists/1.jpg"
                ></v-img>
              </v-list-item-avatar>

              <v-list-item-content>
                <v-list-item-title v-text="user.name"></v-list-item-title>
              </v-list-item-content>
              <v-list-item-icon>
                <v-icon :color="activechat ? 'deep-purple accent-4' : 'grey'">
                  mdi-message-outline
                </v-icon>
              </v-list-item-icon>
            </v-list-item>
          </v-list-item-group>
        </v-list>
      </v-col>
      <v-col v-if="selectedUser">
        <v-container >
          <div v-for="message in messages" :key="message" >
            <v-row
            class="d-flex justify-start pr-6 pa-3"
              v-if="
                message.user == users[selectedUser].id &&
                message.isRecived == 'true'
              "

              
            >
              <v-avatar size="25px">
                <img
                  alt="Avatar"
                  src="https://avatars0.githubusercontent.com/u/9064066?v=4&s=460"
                />
              </v-avatar>
              <!--  confusing naming below -->

              <v-textarea
                :value="message.message"
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
                message.user == users[selectedUser].id &&
                message.isRecived == 'false'
              "
              
            >
              
              <!--  confusing naming below -->

              <v-textarea  
                :value="message.message"
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
                  src="https://cdn.vuetifyjs.com/images/lists/4.jpg"
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
      userMessage: "",
      //messages: [],
      selectedUser: null,
      recent: [
        {
          active: true,
          avatar: "https://cdn.vuetifyjs.com/images/lists/1.jpg",
          title: "Jason Oner",
        },
        {
          active: true,
          avatar: "https://cdn.vuetifyjs.com/images/lists/2.jpg",
          title: "Mike Carlson",
        },
        {
          avatar: "https://cdn.vuetifyjs.com/images/lists/3.jpg",
          title: "Cindy Baker",
        },
        {
          avatar: "https://cdn.vuetifyjs.com/images/lists/4.jpg",
          title: "Ali Connors",
        },
      ],
      activechat: false,
    };
  },
  computed: {
    ...mapGetters(["loggedInUser"]),
    //...mapState({      connection: state => state.connection
    // })
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
              isRecived: "false",
            };
            this.messageContent = ''
            this.messages.push(messageObj);
            console.log(res);
            
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
          isRecived: "true",
        };
        this.messages.push(messageObj);
        console.log(res);
      });
    },
    getUsers: async function () {
      let users = await this.$axios.$get("/api/Users/Users/");
      this.users = users;
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
          this.getUsers();
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
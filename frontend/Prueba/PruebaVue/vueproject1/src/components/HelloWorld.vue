<template>

    <div class="post">
        <div v-if="loading" class="loading">
            Loading... Please refresh once the ASP.NET backend has started. See <a href="https://aka.ms/jspsintegrationvue">https://aka.ms/jspsintegrationvue</a> for more details.
        </div>
        <h1>Here is a child component!</h1>
        <PruebaClass title="ALGO" Fi= "F G"></PruebaClass>
        <div v-if="vUser" >
            <table>
                <thead>
                    <tr>
                        <th>Date</th>
                    </tr>
                </thead>
                <tbody>
                    <tr v-for="user in post" :key="user.username">
                        <td>{{ user.username }}</td>
                    </tr>
                </tbody>
            </table>

        </div>
        <div>
            <input type="text" v-model="name" placeholder="Text">
            <input type="password" v-model="pass" placeholder="Password">
            <button @click="AuthUser">Login</button>
            <div v-if="vProj" class="content" >
                <ul>

                    <ProjectTemplate v-for="project  in projects"
                                     :key= project.id

                                     v-bind.item =project
                                     v-bind:name=project.name
                                     v-on:GetFiles="GetFiles(project.id)">
                    </ProjectTemplate>
                </ul>


            </div>
        </div>
    </div>
</template>
<script setup lang="ts">
    // eslint-disable-next-line
    import PruebaClass from './PruebaClass.vue'
</script>
    <script lang = "ts" >
    import { defineComponent } from 'vue';
        import ProjectTemplate from  './ProjectTemplate.vue'
    type Project = {
        name: string,
        id: string
    };

    type User = {
        username: string,
        password: string,
        projects: Project[]
    };
    type FileToAnalize = {
        name: string;
        id: string;
    }
    interface Data {
        loading: boolean,
        post: null | User,
        name: "",
        pass: "",
        logued: boolean,
        Analizing: boolean,
        Projectid: "",
        vProj: boolean,
        vUser: boolean,
        fileToAnalize: null | FileToAnalize

    }

    export default defineComponent({
        data(): Data {
            return {
                loading: false,
                post: null,
                name: "",
                pass: "",
                logued: false,
                Analizing: false,
                Projectid: "",
                vProj: false,
                vUser: true,
                fileToAnalize: null
            };
        },
        created() {
            // fetch the data when the view is created and the data is
            // already being observed
            this.fetchData();
        },
        watch: {
            // call again the method if the route changes
            '$route': 'fetchData'
        },
        methods: {
            fetchData(): void {
                this.post = null;
                this.loading = true;

                fetch('User')
                    .then(r => r.json())
                    .then(json => {
                        this.post = json as User;
                        this.loading = false;
                        return;
                    });
            },
            AuthUser(): void {
                this.post = null;
                this.loading = true;
                fetch('User/authenticate', {
                    method: 'post',
                    headers: {
                        'Content-Type': 'application/json'
                    },
                    body: JSON.stringify({ "username": this.$data.name, "password": this.$data.pass })
                }).then(r => r.json())
                    .then(json => {
                        this.post = json as User;
                        this.logued = true;
                        this.loading = false;
                        this.vProj = true;

                        return;
                    });
            },
            GetFiles(Id: string): void {
                this.vProj = false;

                this.post = null;
                this.loading = true;
                fetch('File/DataToAnalize/?ProyectID=' + JSON.stringify(Id), {

                    method: 'get',
                    headers: {

                        'Authorization': 'YXN5bmNBZ2FpbjoxMjM0NQ=='
                    }
                }).then(r => r.json())
                    .then(json => {
                        this.fileToAnalize = json as FileToAnalize;
                        this.logued = false;
                        this.loading = false;
                        this.Analizing = true;
                        return;
                    });
            }
        },
    });
</script>
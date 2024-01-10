<template>

    <div class="post">
        <div v-if="loading" class="loading">
            Loading... Please refresh once the ASP.NET backend has started. See <a href="https://aka.ms/jspsintegrationvue">https://aka.ms/jspsintegrationvue</a> for more details.
        </div>

        <div v-if="loading" class="content">
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
            <div v-if="logued" class="content">
                <table>
                    <thead>
                        <tr>
                            <th>name</th>
                        </tr>
                    </thead>
                    <tbody>
                        <tr v-for="project in post.projects" :key="project.id">
                            <td>

                                <a class="badge badge-warning"
                                   :href="'/tutorials/' + project.id"> {{ project.name }}</a>
                            </td></tr>
                    </tbody>
                </table>

            </div>
        </div>
    </div>
</template>

<script lang="ts">
    import { defineComponent } from 'vue';
    type Project = {
        name: string,
        id: string
    };

    type User = {
        username: string,
        password: string,
        projects: Project[] 
    };

    interface Data {
        loading: boolean,
        post: null | User,
        name: "",
        pass: "",
        logued: boolean,
        Analizing: boolean,
        Projectid: ""
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
                Projectid:""
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
                        return;
                    });
            },
            GetFiles(Id:string ): void {
                this.post = null;
                this.loading = true;
                fetch('Files/GetAnalizedFiles', {
                    method: 'get',
                    headers: {
                        'Content-Type': 'application/json'
                    },
                    body: JSON.stringify({ "ProyectID": Id })
                }).then(r => r.json())
                    .then(json => {
                        this.post = json as User;
                        this.logued = false;
                        this.loading = false;
                        this.Analizing = true;
                        return;
                    });
            }
        },
    });
</script>
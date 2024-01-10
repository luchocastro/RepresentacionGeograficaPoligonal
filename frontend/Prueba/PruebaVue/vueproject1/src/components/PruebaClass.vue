<script setup lang="ts">
    import { ref, computed, defineProps } from 'vue';


    const firstName = ref('John');
    const lastName = ref('Doe');
    

    const fullName = computed <string>({
        // getter
        get() {
            return firstName.value + ' ' + lastName.value;
        },
        // setter
        set(newValue) {
           var a:string [];
            a = newValue.split(' ')
            firstName.value = a[0]
            lastName.value = a[1];
        }
    });
    const count = ref(0);
    const props = defineProps<{
        title?: string
        likes?: number
    }>()
</script>

<template>
    <h5>{{props.title }}</h5>
    <h5>{{ fullName.value  }}</h5>
    
    <button @click="count++">You clicked me {{ count }} times.</button>
</template>
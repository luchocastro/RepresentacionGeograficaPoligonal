
<script setup>

    import { defineComponent } from 'vue'
    export default defineComponent({
        props: {

            name: { type: String },
            
            id: { type: String }

        }
        
    });
defineEmits(['GetFiles'])
    
</script>

<template
          
     v-for="item  in items"
    v-bind:item= item
    v-bind:id=item.id  
    v-bind:key="item.id"
          >

</template>
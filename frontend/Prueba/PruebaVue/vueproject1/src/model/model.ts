import { defineComponent } from 'vue'
import type { PropType } from 'vue'

interface User {
    username: string
    password: string
    id: string
    projects: NamedObject[]
}

interface NamedObject{
    name: string
    id: string
}
interface Column {
    name: string
    datatype: string
    position: number
}
 
export default defineComponent({
    props: {
        user: {
            // provide more specific type to `Object`
            type: Object as PropType<User>,
            required: true
        }} 
})
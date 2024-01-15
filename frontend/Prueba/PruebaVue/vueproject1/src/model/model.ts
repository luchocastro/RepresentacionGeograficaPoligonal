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
    key: string
    parent: string
}
interface Column {
    name: string
    datatype: string
    position: number
}
 
export default interface Named {
        
            name: string
            id: string
            parent: string
        }

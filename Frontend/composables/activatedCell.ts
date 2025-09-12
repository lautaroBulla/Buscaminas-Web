import { ref } from 'vue';

const activatedCell = ref(null);

export function useActivatedCell() {
  return {
    activatedCell
  };
}
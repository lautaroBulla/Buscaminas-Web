<script setup>
  //componente que muestra una animacion de carga
  import { ref, onMounted, computed } from 'vue';
  import mineDark from '~/public/img/themes/dark/mineDark.png';
  import mineLight from '~/public/img/themes/light/mineLight.png';

  const props = defineProps({
    width: {
      type: String,
      required: true
    },
    height: {
      type: String,
      required: true
    }
  });

  const colorMode = useColorMode();
  const isDark = computed(() => colorMode.value === 'dark');

  const gridSize = 9;
  const delayBase = 100;
  const revealedPixels = ref(new Set());

  // const pixels = Array.from({ length: gridSize * gridSize }, (_, i) => {
  //   const row = Math.floor(i / gridSize);
  //   const col = i % gridSize;
  //   const center = (gridSize - 1) / 2;
  //   const distance = Math.sqrt((row - center) ** 2 + (col - center) ** 2);

  //   const pixelNot = [
  //     [0, 0], [0, 1], [0, 2], [0, 3], [0, 6], [0, 5], [0, 7], [0, 8],
  //     [1, 0], [1, 2], [1, 3], [1, 5], [1, 6], [1, 8],
  //     [2, 0], [2, 1], [2, 2], [2, 6], [2, 7], [2, 8],
  //     [3, 0], [3, 1], [3, 7], [3, 8],
  //     [5, 0], [5, 1], [5, 7], [5, 8],
  //     [6, 0], [6, 1], [6, 2], [6, 6], [6, 7], [6, 8],
  //     [7, 0], [7, 2], [7, 3], [7, 5], [7, 6], [7, 8],
  //     [8, 0], [8, 1], [8, 2], [8, 3], [8, 6], [8, 5], [8, 7], [8, 8],
  //   ];

  //   if (pixelNot.some(([r, c]) => r === row && c === col)) {
  //     return {
  //       id: i,
  //       row,
  //       col,
  //       delay: distance * delayBase,
  //       show: false
  //     }
  //   } else {
  //     return {
  //       id: i,
  //       row,
  //       col,
  //       delay: distance * delayBase,
  //       show: true
  //     };
  //   }
  // });

  const pixels = [
    {
        "id": 0,
        "row": 0,
        "col": 0,
        "delay": 565.685424949238,
        "show": false
    },
    {
        "id": 1,
        "row": 0,
        "col": 1,
        "delay": 500,
        "show": false
    },
    {
        "id": 2,
        "row": 0,
        "col": 2,
        "delay": 447.21359549995793,
        "show": false
    },
    {
        "id": 3,
        "row": 0,
        "col": 3,
        "delay": 412.31056256176606,
        "show": false
    },
    {
        "id": 4,
        "row": 0,
        "col": 4,
        "delay": 400,
        "show": true
    },
    {
        "id": 5,
        "row": 0,
        "col": 5,
        "delay": 412.31056256176606,
        "show": false
    },
    {
        "id": 6,
        "row": 0,
        "col": 6,
        "delay": 447.21359549995793,
        "show": false
    },
    {
        "id": 7,
        "row": 0,
        "col": 7,
        "delay": 500,
        "show": false
    },
    {
        "id": 8,
        "row": 0,
        "col": 8,
        "delay": 565.685424949238,
        "show": false
    },
    {
        "id": 9,
        "row": 1,
        "col": 0,
        "delay": 500,
        "show": false
    },
    {
        "id": 10,
        "row": 1,
        "col": 1,
        "delay": 424.2640687119285,
        "show": true
    },
    {
        "id": 11,
        "row": 1,
        "col": 2,
        "delay": 360.5551275463989,
        "show": false
    },
    {
        "id": 12,
        "row": 1,
        "col": 3,
        "delay": 316.22776601683796,
        "show": false
    },
    {
        "id": 13,
        "row": 1,
        "col": 4,
        "delay": 300,
        "show": true
    },
    {
        "id": 14,
        "row": 1,
        "col": 5,
        "delay": 316.22776601683796,
        "show": false
    },
    {
        "id": 15,
        "row": 1,
        "col": 6,
        "delay": 360.5551275463989,
        "show": false
    },
    {
        "id": 16,
        "row": 1,
        "col": 7,
        "delay": 424.2640687119285,
        "show": true
    },
    {
        "id": 17,
        "row": 1,
        "col": 8,
        "delay": 500,
        "show": false
    },
    {
        "id": 18,
        "row": 2,
        "col": 0,
        "delay": 447.21359549995793,
        "show": false
    },
    {
        "id": 19,
        "row": 2,
        "col": 1,
        "delay": 360.5551275463989,
        "show": false
    },
    {
        "id": 20,
        "row": 2,
        "col": 2,
        "delay": 282.842712474619,
        "show": false
    },
    {
        "id": 21,
        "row": 2,
        "col": 3,
        "delay": 223.60679774997897,
        "show": true
    },
    {
        "id": 22,
        "row": 2,
        "col": 4,
        "delay": 200,
        "show": true
    },
    {
        "id": 23,
        "row": 2,
        "col": 5,
        "delay": 223.60679774997897,
        "show": true
    },
    {
        "id": 24,
        "row": 2,
        "col": 6,
        "delay": 282.842712474619,
        "show": false
    },
    {
        "id": 25,
        "row": 2,
        "col": 7,
        "delay": 360.5551275463989,
        "show": false
    },
    {
        "id": 26,
        "row": 2,
        "col": 8,
        "delay": 447.21359549995793,
        "show": false
    },
    {
        "id": 27,
        "row": 3,
        "col": 0,
        "delay": 412.31056256176606,
        "show": false
    },
    {
        "id": 28,
        "row": 3,
        "col": 1,
        "delay": 316.22776601683796,
        "show": false
    },
    {
        "id": 29,
        "row": 3,
        "col": 2,
        "delay": 223.60679774997897,
        "show": true
    },
    {
        "id": 30,
        "row": 3,
        "col": 3,
        "delay": 141.4213562373095,
        "show": true
    },
    {
        "id": 31,
        "row": 3,
        "col": 4,
        "delay": 100,
        "show": true
    },
    {
        "id": 32,
        "row": 3,
        "col": 5,
        "delay": 141.4213562373095,
        "show": true
    },
    {
        "id": 33,
        "row": 3,
        "col": 6,
        "delay": 223.60679774997897,
        "show": true
    },
    {
        "id": 34,
        "row": 3,
        "col": 7,
        "delay": 316.22776601683796,
        "show": false
    },
    {
        "id": 35,
        "row": 3,
        "col": 8,
        "delay": 412.31056256176606,
        "show": false
    },
    {
        "id": 36,
        "row": 4,
        "col": 0,
        "delay": 400,
        "show": true
    },
    {
        "id": 37,
        "row": 4,
        "col": 1,
        "delay": 300,
        "show": true
    },
    {
        "id": 38,
        "row": 4,
        "col": 2,
        "delay": 200,
        "show": true
    },
    {
        "id": 39,
        "row": 4,
        "col": 3,
        "delay": 100,
        "show": true
    },
    {
        "id": 40,
        "row": 4,
        "col": 4,
        "delay": 0,
        "show": true
    },
    {
        "id": 41,
        "row": 4,
        "col": 5,
        "delay": 100,
        "show": true
    },
    {
        "id": 42,
        "row": 4,
        "col": 6,
        "delay": 200,
        "show": true
    },
    {
        "id": 43,
        "row": 4,
        "col": 7,
        "delay": 300,
        "show": true
    },
    {
        "id": 44,
        "row": 4,
        "col": 8,
        "delay": 400,
        "show": true
    },
    {
        "id": 45,
        "row": 5,
        "col": 0,
        "delay": 412.31056256176606,
        "show": false
    },
    {
        "id": 46,
        "row": 5,
        "col": 1,
        "delay": 316.22776601683796,
        "show": false
    },
    {
        "id": 47,
        "row": 5,
        "col": 2,
        "delay": 223.60679774997897,
        "show": true
    },
    {
        "id": 48,
        "row": 5,
        "col": 3,
        "delay": 141.4213562373095,
        "show": true
    },
    {
        "id": 49,
        "row": 5,
        "col": 4,
        "delay": 100,
        "show": true
    },
    {
        "id": 50,
        "row": 5,
        "col": 5,
        "delay": 141.4213562373095,
        "show": true
    },
    {
        "id": 51,
        "row": 5,
        "col": 6,
        "delay": 223.60679774997897,
        "show": true
    },
    {
        "id": 52,
        "row": 5,
        "col": 7,
        "delay": 316.22776601683796,
        "show": false
    },
    {
        "id": 53,
        "row": 5,
        "col": 8,
        "delay": 412.31056256176606,
        "show": false
    },
    {
        "id": 54,
        "row": 6,
        "col": 0,
        "delay": 447.21359549995793,
        "show": false
    },
    {
        "id": 55,
        "row": 6,
        "col": 1,
        "delay": 360.5551275463989,
        "show": false
    },
    {
        "id": 56,
        "row": 6,
        "col": 2,
        "delay": 282.842712474619,
        "show": false
    },
    {
        "id": 57,
        "row": 6,
        "col": 3,
        "delay": 223.60679774997897,
        "show": true
    },
    {
        "id": 58,
        "row": 6,
        "col": 4,
        "delay": 200,
        "show": true
    },
    {
        "id": 59,
        "row": 6,
        "col": 5,
        "delay": 223.60679774997897,
        "show": true
    },
    {
        "id": 60,
        "row": 6,
        "col": 6,
        "delay": 282.842712474619,
        "show": false
    },
    {
        "id": 61,
        "row": 6,
        "col": 7,
        "delay": 360.5551275463989,
        "show": false
    },
    {
        "id": 62,
        "row": 6,
        "col": 8,
        "delay": 447.21359549995793,
        "show": false
    },
    {
        "id": 63,
        "row": 7,
        "col": 0,
        "delay": 500,
        "show": false
    },
    {
        "id": 64,
        "row": 7,
        "col": 1,
        "delay": 424.2640687119285,
        "show": true
    },
    {
        "id": 65,
        "row": 7,
        "col": 2,
        "delay": 360.5551275463989,
        "show": false
    },
    {
        "id": 66,
        "row": 7,
        "col": 3,
        "delay": 316.22776601683796,
        "show": false
    },
    {
        "id": 67,
        "row": 7,
        "col": 4,
        "delay": 300,
        "show": true
    },
    {
        "id": 68,
        "row": 7,
        "col": 5,
        "delay": 316.22776601683796,
        "show": false
    },
    {
        "id": 69,
        "row": 7,
        "col": 6,
        "delay": 360.5551275463989,
        "show": false
    },
    {
        "id": 70,
        "row": 7,
        "col": 7,
        "delay": 424.2640687119285,
        "show": true
    },
    {
        "id": 71,
        "row": 7,
        "col": 8,
        "delay": 500,
        "show": false
    },
    {
        "id": 72,
        "row": 8,
        "col": 0,
        "delay": 565.685424949238,
        "show": false
    },
    {
        "id": 73,
        "row": 8,
        "col": 1,
        "delay": 500,
        "show": false
    },
    {
        "id": 74,
        "row": 8,
        "col": 2,
        "delay": 447.21359549995793,
        "show": false
    },
    {
        "id": 75,
        "row": 8,
        "col": 3,
        "delay": 412.31056256176606,
        "show": false
    },
    {
        "id": 76,
        "row": 8,
        "col": 4,
        "delay": 400,
        "show": true
    },
    {
        "id": 77,
        "row": 8,
        "col": 5,
        "delay": 412.31056256176606,
        "show": false
    },
    {
        "id": 78,
        "row": 8,
        "col": 6,
        "delay": 447.21359549995793,
        "show": false
    },
    {
        "id": 79,
        "row": 8,
        "col": 7,
        "delay": 500,
        "show": false
    },
    {
        "id": 80,
        "row": 8,
        "col": 8,
        "delay": 565.685424949238,
        "show": false
    }
 ];

  const startAnimation = () => {
    pixels.forEach(pixel => {
      if (pixel.show) {
        setTimeout(() => {
          revealedPixels.value.add(pixel.id);
        }, pixel.delay);
      }
    });
    
    const maxDelay = Math.max(...pixels.filter(p => p.show).map(p => p.delay));
    
    setTimeout(() => {
      revealedPixels.value.clear();
      setTimeout(startAnimation, 500);
    }, maxDelay + 300);
  };

  onMounted(() => {
    startAnimation();
  });

  const gridWidth = computed(() => {
    const pixelSize = parseInt(props.width);
    return gridSize * pixelSize;
  });
</script>

<template>
  <div class="flex items-center justify-center gap-x-4">
    <p>{{ $t('loading.loading') }}</p>

    <div 
      class="grid" 
      :style="`grid-template-columns: repeat(${gridSize}, 1fr); width: ${gridWidth}px; height: ${gridWidth}px;`"
    >
      <div v-for="pixel in pixels" :key="pixel.id">
        <Transition name="reveal" appear>
          <img 
            v-if="pixel.show && revealedPixels.has(pixel.id)" 
            :src="isDark ? mineDark : mineLight" 
            :class="`w-[${width}] h-[${height}]`"
          />
        </Transition>
      </div>
    </div>

  </div>
</template>

<style scoped>
.reveal-enter-active {
  transition: all 0.3s ease;
}
.reveal-enter-from {
  opacity: 0;
  transform: scale(0);
}
.reveal-enter-to {
  opacity: 1;
  transform: scale(1);
}
</style>
<script setup>
  //componente para cada celda del tablero
  import oneClasssic from '~/assets/img/themes/classicTheme/one.png';
  import twoClasssic from '~/assets/img/themes/classicTheme/two.png';
  import threeClasssic from '~/assets/img/themes/classicTheme/three.png';
  import fourClasssic from '~/assets/img/themes/classicTheme/four.png';
  import fiveClasssic from '~/assets/img/themes/classicTheme/five.png';
  import sixClasssic from '~/assets/img/themes/classicTheme/six.png';
  import sevenClasssic from '~/assets/img/themes/classicTheme/seven.png';
  import eightClasssic from '~/assets/img/themes/classicTheme/eight.png';
  import mineClasssic from '~/assets/img/themes/classicTheme/mine.png';
  import flagClasssic from '~/assets/img/themes/classicTheme/flag.png';
  import interrogationClasssic from '~/assets/img/themes/classicTheme/interrogation.png';

  import oneDark from '~/assets/img/themes/darkTheme/oneDark.png';
  import twoDark from '~/assets/img/themes/darkTheme/twoDark.png';
  import threeDark from '~/assets/img/themes/darkTheme/threeDark.png';
  import fourDark from '~/assets/img/themes/darkTheme/fourDark.png';
  import fiveDark from '~/assets/img/themes/darkTheme/fiveDark.png';
  import sixDark from '~/assets/img/themes/darkTheme/sixDark.png';
  import sevenDark from '~/assets/img/themes/darkTheme/sevenDark.png';
  import eightDark from '~/assets/img/themes/darkTheme/eightDark.png';
  import mineDark from '~/assets/img/themes/darkTheme/mineDark.png';
  import flagDark from '~/assets/img/themes/darkTheme/flagDark.png';
  import interrogationDark from '~/assets/img/themes/darkTheme/interrogationDark.png';
  import { computed, ref } from 'vue';

  const props = defineProps({
    cell: {
      type: [Number, String],
      required: true
    },
    rowIndex: {
      type: Number,
      required: true
    },
    colIndex: {
      type: Number,
      required: true
    },
    reveal: {
      type: Boolean,
      required: true
    },
    flag: {
      type: Boolean,
      required: true
    },
    interrogation: {
      type: Boolean,
      required: true
    },
    exploted: {
      type: Boolean
    },
    isHelpCell: {
      type: Boolean
    }
  });

  const { currentTheme } = useCurrentTheme();
  const { isMobile } = useIsMobile();
  const { activatedCell } = useActivatedCell();

	const imgByTheme = {
		classicTheme: {
			one: oneClasssic,
      two: twoClasssic,
      three: threeClasssic,
      four: fourClasssic,
      five: fiveClasssic,
      six: sixClasssic,
      seven: sevenClasssic,
      eight: eightClasssic,
      mine: mineClasssic,
      flag: flagClasssic,
      interrogation: interrogationClasssic
		},
    darkTheme: {
      one: oneDark,
      two: twoDark,
      three: threeDark,
      four: fourDark,
      five: fiveDark,
      six: sixDark,
      seven: sevenDark,
      eight: eightDark,
      mine: mineDark,
      flag: flagDark,
      interrogation: interrogationDark
    }
  };

  const currentThemeComputed = computed(() => {
    return currentTheme.value;
  });

  const emit = defineEmits(['left-click', 'rigth-click']);

  //pc click izquierdo
  const handleLeftClick = () => {
    emit('left-click', {row: props.rowIndex, col: props.colIndex});
  }
  /* 
  pc click derecho
  e se utliza para prevenir el menu contextual del click derecho*/
  const handleRightClick = (e) => {
    e.preventDefault();
    emit('rigth-click', {row: props.rowIndex, col: props.colIndex});
  }

  /*mobile
  touchstart se utiliza para detectar el inicio del toque
  touchend se utiliza para detectar el final del toque
  touchcancel se utiliza para detectar cuando el toque es cancelado
  */
  const cellId = `${props.rowIndex}-${props.colIndex}`;
  let timer = null;
  const changeCell = ref(false);
  const isTouched = ref(false);
  const handleTouchStart = (e) => {
    if (e.touches.length > 1) {
      if (activatedCell.value === cellId) {
        handleTouchCancel();
      }
      return; 
    }

    if (activatedCell.value !== null && activatedCell.value !== cellId) {
      return;
    } else {
      // e.preventDefault();

      activatedCell.value = cellId;
      isTouched.value = true;

      timer = setTimeout(() => {
        if (activatedCell.value === cellId && isTouched.value) {
          changeCell.value = true;
          emitEvent(); 
        }
      }, 250);
    }
  }

  const handleTouchMove = (e) => {
    if (e.touches.length > 1) {
      handleTouchCancel();
      return;
    }

    if (isTouched.value && activatedCell.value === cellId) {
      const touch = e.touches[0];
      const element = e.currentTarget;
      const rect = element.getBoundingClientRect();
      
      const isInsideCell = (
        touch.clientX >= rect.left &&
        touch.clientX <= rect.right &&
        touch.clientY >= rect.top &&
        touch.clientY <= rect.bottom
      );
      
      if (!isInsideCell) {
        handleTouchCancel();
      }
    }
  }

  const handleTouchEnd = () => {
    if (isTouched.value && activatedCell.value === cellId) {
      clearTimeout(timer);
      if (!changeCell.value) {
        emitEvent();
      }
      
      handleTouchCancel();
    }
  }

  const emitEvent = () => {
    if (changeCell.value) {
      emit('rigth-click', {row: props.rowIndex, col: props.colIndex});
    } else {
      emit('left-click', {row: props.rowIndex, col: props.colIndex});
    }
  }

  const handleTouchCancel = () => {
    isTouched.value = false;
    clearTimeout(timer);
    changeCell.value = false;
    if (activatedCell.value === cellId) {
      activatedCell.value = null;
    }
  }
</script>

<template>
    <div
      ref="cellRef"
      :class="[
        reveal ? 'reveal' : 'cell',
        exploted ? 'reveal-lose' : '',
        isHelpCell ? 'help' : '',
        isTouched ? 'is-active' : ''
      ]"
      @click="!isMobile ? handleLeftClick() : null"
      @contextmenu="!isMobile ? handleRightClick($event) : null"
      @touchstart="isMobile ? handleTouchStart($event) : null"
      @touchmove="isMobile ? handleTouchMove($event) : null"
      @touchend="isMobile ? handleTouchEnd() : null"
      @touchcancel="isMobile ? handleTouchCancel() : null"
    >
      <img v-if="reveal && cell === 1" :src="imgByTheme[currentThemeComputed].one" alt="One" class="pointer-events-none" />
      <img v-else-if="reveal && cell === 2" :src="imgByTheme[currentThemeComputed].two" alt="Two" class="pointer-events-none" />
      <img v-else-if="reveal && cell === 3" :src="imgByTheme[currentThemeComputed].three" alt="Three" class="pointer-events-none" />
      <img v-else-if="reveal && cell === 4" :src="imgByTheme[currentThemeComputed].four" alt="Four" class="pointer-events-none" />
      <img v-else-if="reveal && cell === 5" :src="imgByTheme[currentThemeComputed].five" alt="Five" class="pointer-events-none" />
      <img v-else-if="reveal && cell === 6" :src="imgByTheme[currentThemeComputed].six" alt="Six" class="pointer-events-none" />
      <img v-else-if="reveal && cell === 7" :src="imgByTheme[currentThemeComputed].seven" alt="Seven" class="pointer-events-none" />
      <img v-else-if="reveal && cell === 8" :src="imgByTheme[currentThemeComputed].eight" alt="Eight" class="pointer-events-none" />
      <img v-else-if="reveal && cell === 'M'" :src="imgByTheme[currentThemeComputed].mine" alt="Mine" class="pointer-events-none" />

      <!-- Si no revelada -->
      <img v-else-if="!reveal && flag" :src="imgByTheme[currentThemeComputed].flag" alt="Flag" class="pointer-events-none" />
      <img v-else-if="!reveal && interrogation" :src="imgByTheme[currentThemeComputed].interrogation" alt="Interrogation" class="pointer-events-none" />  
    </div>
</template>
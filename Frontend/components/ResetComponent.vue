<script setup>
	//componente para reinciar el juego
	import faceClassic from '~/assets/img/themes/classicTheme/face.png';
	import faceWinClassic from '~/assets/img/themes/classicTheme/faceWin.png';
	import faceLoseClassic from '~/assets/img/themes/classicTheme/faceLose.png';

	import faceDark from '~/assets/img/themes/darkTheme/faceDark.png';
	import faceWinDark from '~/assets/img/themes/darkTheme/faceWinDark.png';
	import faceLoseDark from '~/assets/img/themes/darkTheme/faceLoseDark.png';
	
	const props = defineProps({
		gameOver: {
			type: Boolean,
      required: true
    },
    gameWin: {
      type: Boolean,
      required: true
    }
  })

	const { currentTheme } = useCurrentTheme();
	
	const imgByTheme = {
		classicTheme: {
			face: faceClassic,
			faceWin: faceWinClassic,
			faceLose: faceLoseClassic
		},
		darkTheme: {
			face: faceDark,
			faceWin: faceWinDark,
			faceLose: faceLoseDark
		}
  };
	
	const faceImage = computed(() => {
		if (props.gameOver) {
			return imgByTheme[currentTheme.value].faceLose;
		} else if (props.gameWin) {
			return imgByTheme[currentTheme.value].faceWin;
		} else {
			return imgByTheme[currentTheme.value].face;
		}
	});
	
	const faceAlt = computed(() => {
		if (props.gameOver) {
			return 'Game Over';
		} else if (props.gameWin) {
			return 'You Win';
		} else {
			return 'Restart Game';
		}
	});
</script>

<template>
	<div class="button-border">
		<button @click="$emit('restart-game')" 
						class="button w-[38px] h-[38px]"
						:title="$t('header.reset')"
		>
			<img :src="faceImage" :alt="faceAlt"/>
		</button>
	</div>
</template>